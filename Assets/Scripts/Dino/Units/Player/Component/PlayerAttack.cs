﻿using System;
using System.Collections.Generic;
using System.Linq;
using Dino.Extension;
using Dino.Units.Component;
using Dino.Units.Component.Animation;
using Dino.Units.Component.Health;
using Dino.Units.Component.Target;
using Dino.Units.Component.TargetSearcher;
using Dino.Units.Player.Model;
using Dino.Weapon;
using Dino.Weapon.Components;
using Dino.Weapon.Model;
using Feofun.Components;
using JetBrains.Annotations;
using Logger.Extension;
using ModestTree;
using SuperMaxim.Core.Extensions;
using UnityEngine;

namespace Dino.Units.Player.Component
{
    [RequireComponent(typeof(ITargetSearcher))]
    [RequireComponent(typeof(MovementController))]
    public class PlayerAttack : MonoBehaviour, IUpdatableComponent
    {
        private static readonly int AttackSpeedMultiplierHash = Animator.StringToHash("AttackSpeedMultiplier");
        private static readonly int AttackHash = Animator.StringToHash("Attack");

        [SerializeField]
        private bool _rotateToTarget = true;
        [SerializeField]
        private string _attackAnimationName;
        
        private Animator _animator;
        private AnimationSwitcher _animationSwitcher;
        private ITargetSearcher _targetSearcher;
        private MovementController _movementController;

        private List<IInitializable<IWeaponModel>> _weaponDependentComponents;
        [CanBeNull]
        private ChangeableWeapon _weapon;
        [CanBeNull]
        private WeaponAnimationHandler _weaponAnimationHandler;
        [CanBeNull]
        private ITarget _target;

        private bool IsTargetInvalid => !_target.IsTargetValidAndAlive();
        private bool HasWeaponAnimationHandler => _weaponAnimationHandler != null;

        private void Awake()
        {
            _animator = gameObject.RequireComponentInChildren<Animator>();
            _animationSwitcher = gameObject.RequireComponentInChildren<AnimationSwitcher>();
            _targetSearcher = GetComponent<ITargetSearcher>();
            _movementController = GetComponent<MovementController>();
            _weaponDependentComponents = GetComponentsInChildren<IInitializable<IWeaponModel>>().ToList();
            _weaponAnimationHandler = GetComponentInChildren<WeaponAnimationHandler>();
            if (HasWeaponAnimationHandler) {
                _weaponAnimationHandler.OnFireEvent += Fire;
            }
        }

        public void SetWeapon(PlayerWeaponModel weaponModel, BaseWeapon weapon)
        {
            Assert.IsNull(_weapon, $"Player weapon is not null, should delete the previous weapon");
            _weapon = new ChangeableWeapon() {
                    Weapon = weapon,
                    Model = weaponModel,
                    Timer = new WeaponTimer(weaponModel.AttackInterval),
            };
            InitWeaponDependentComponents(weaponModel);
            OverrideAnimation(weaponModel.Animation);
            UpdateAnimationSpeed(weaponModel.AttackInterval, weaponModel.Animation);
        }

        private void InitWeaponDependentComponents(IWeaponModel weaponModel)
        {
            var playerSearcher = _targetSearcher as IInitializable<IWeaponModel>;
            if (playerSearcher == null) {
                this.Logger().Error("Target searcher on player must be IInitializable<IWeaponModel>");
                return;
            } 
            playerSearcher.Init(weaponModel);
            _weaponDependentComponents.Except(playerSearcher);
            _weaponDependentComponents.ForEach(it => it.Init(weaponModel));
        }

        private void OverrideAnimation(string animationId)
        {
            _animationSwitcher.OverrideAnimation(_attackAnimationName, animationId);
        }

        public void DeleteWeapon()
        {
            _weapon = null;
            if (_rotateToTarget) {
                _movementController.RotateToTarget(null);
            }

            foreach (var component in _weaponDependentComponents)
            {
                var disposable = component as IDisposable;
                disposable?.Dispose();
            }
        }

        private void UpdateAnimationSpeed(float attackInterval, string animationId)
        {
            var clips = _animator.runtimeAnimatorController.animationClips;
            var attackClipLength = clips.First(it => it.name == _animationSwitcher.GetAnimationName(animationId)).length;
            if (attackInterval >= attackClipLength) {
                return;
            }
            _animator.SetFloat(AttackSpeedMultiplierHash, attackClipLength / attackInterval);
        }

        [CanBeNull]
        private ITarget FindTarget() => _targetSearcher.Find();

        public void OnTick()
        {
            if (_weapon == null) {
                return;
            }
            var target = FindTarget();
            if (_rotateToTarget) {
                _movementController.RotateToTarget(target?.Center);
            }
            if (CanAttack(target)) {
                Attack(target);
            }
        }

        private bool CanAttack([CanBeNull] ITarget target) => _weapon != null && target != null && _weapon.Timer.IsAttackReady;

        private void Attack(ITarget target)
        {
            if (_weapon == null) {
                this.Logger().Error("Weapon is not setted");
                return;
            }
            _target = target;
            _animator.SetTrigger(AttackHash);
            _weapon.Timer.OnAttack();
            if (!HasWeaponAnimationHandler) {
                Fire();
            }
        }

        private void Fire()
        {
            if (_weapon == null) {
                this.Logger().Warn("Weapon removed while fire");
                return;
            }
            if (IsTargetInvalid) {
                _weapon.Timer.SetAttackAsReady();
                return;
            }
            _weapon.Fire(_target, DoDamage);
        }
        
        private void DoDamage(GameObject target)
        {
            if (_weapon == null) {
                this.Logger().Warn("Weapon removed while fire");
                return;
            }
            var damageable = target.RequireComponent<IDamageable>();
            damageable.TakeDamage(_weapon.Model.AttackDamage);
            this.Logger().Trace($"Damage applied, target:= {target.name}");
        }

        private void OnDestroy()
        {
            DeleteWeapon();
            if (HasWeaponAnimationHandler) {
                _weaponAnimationHandler.OnFireEvent -= Fire;
            }
        }

        private void OnDrawGizmos()
        {
            if (_weapon == null) {
                return;
            }
            DrawAttackDistance();
        }
        private void DrawAttackDistance()
        {
            var color = Color.green;
            color.a /= 4;
            Gizmos.color = color;
            Gizmos.DrawSphere(transform.position, _weapon.Model.AttackDistance);
        }

        private class ChangeableWeapon
        {
            public BaseWeapon Weapon { get; set; }
            public IWeaponModel Model { get; set; }
            public WeaponTimer Timer { get; set; }

            public void Fire(ITarget target, Action<GameObject> hitCallback)
            {
                Weapon.Fire(target, Model, hitCallback);
            }
        }
    }
}