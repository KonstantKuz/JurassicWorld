using System;
using System.Collections.Generic;
using System.Linq;
using Dino.Extension;
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
using UnityEngine;

namespace Dino.Units.Player.Component
{
    [RequireComponent(typeof(ITargetSearcher))]
    [RequireComponent(typeof(MovementController))]
    public class PlayerAttack : MonoBehaviour, IUpdatableComponent, IInitializable<Unit>
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
        private bool _startedAttack;
        private bool _shootOnMove;

        [CanBeNull]
        private WeaponWrapper _weapon;
        [CanBeNull]
        private WeaponAnimationHandler _weaponAnimationHandler;
        [CanBeNull]
        private ITarget _target;
        private bool IsTargetInvalid => !_target.IsTargetValidAndAlive();
        private bool HasWeaponAnimationHandler => _weaponAnimationHandler != null;
        
        [CanBeNull]
        public WeaponWrapper WeaponWrapper => _weapon;
        
        private bool IsAttackAllowedByWeapon => _weapon != null && _weapon.IsWeaponReadyToFire;
        private bool IsAttackAllowedByMovement => !_movementController.IsMoving || _shootOnMove;

        public event Action OnAttacked;

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
        public void Init(Unit unit)
        {
            if (!(unit.Model is PlayerUnitModel model)) {
                throw new InvalidCastException($"The player model must be of type:= {nameof(PlayerUnitModel)}");
            }
            _shootOnMove = model.ShootOnMove;
        }
        public void SetWeapon(WeaponWrapper changeableWeapon)
        {
            Assert.IsNull(_weapon, $"Player weapon is not null, should delete the previous weapon");
            _weapon = changeableWeapon;
            InitWeaponDependentComponents(_weapon.Model);
            OverrideAnimation(_weapon.Model.Animation);
            UpdateAnimationSpeed(_weapon.Model.AttackInterval, _weapon.Model.Animation);
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
            _startedAttack = false;
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

        private bool CanAttack([CanBeNull] ITarget target) => target != null 
                                                              && !_startedAttack &&
                                                              IsAttackAllowedByWeapon &&
                                                              IsAttackAllowedByMovement;



        private void Attack(ITarget target)
        {
            if (_weapon == null) {
                this.Logger().Error("Weapon is not setted");
                return;
            }
            _target = target;
            _animator.SetTrigger(AttackHash);
            _startedAttack = true;
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
            if (!_startedAttack) {
                return;
            }
            _weapon.Fire(_target, DoDamage);
            _weapon.Timer.OnAttack();
            _startedAttack = false;
            OnAttacked?.Invoke();
        }
        
        private void DoDamage(GameObject target)
        {
            if (_weapon == null) {
                this.Logger().Warn("Weapon removed while fire");
                return;
            }
            var damageable = target.RequireComponent<IDamageable>();
            var damageParams = new HitParams {
                Damage = _weapon.Model.AttackDamage,
                AttackerPosition = transform.position
            };
            damageable.TakeDamage(damageParams);
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

    }
}