using System.Linq;
using Dino.Extension;
using Dino.Units.Component.Health;
using Dino.Units.Component.TargetSearcher;
using Dino.Units.Player.Movement;
using Dino.Units.Target;
using Dino.Weapon;
using Dino.Weapon.Model;
using Feofun.Components;
using JetBrains.Annotations;
using Logger.Extension;
using UnityEngine;

namespace Dino.Units.Player.Attack
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
        private ITargetSearcher _targetSearcher;
        private MovementController _movementController;

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
            _targetSearcher = GetComponent<ITargetSearcher>();
            _movementController = GetComponent<MovementController>();
            _weaponAnimationHandler = GetComponentInChildren<WeaponAnimationHandler>();
            if (HasWeaponAnimationHandler) {
                _weaponAnimationHandler.OnFireEvent += Fire;
            }
        }

        public void SetWeapon(IWeaponModel weaponModel, BaseWeapon weapon)
        {
            _weapon = new ChangeableWeapon() {
                    Weapon = weapon,
                    Model = weaponModel,
                    Timer = new WeaponTimer(weaponModel.AttackInterval),
            };
            UpdateAnimationSpeed(weaponModel.AttackInterval);
        }

        public void DeleteWeapon() => _weapon = null;

        private void UpdateAnimationSpeed(float attackInterval)
        {
            var clips = _animator.runtimeAnimatorController.animationClips;
            var attackClipLength = clips.First(it => it.name == _attackAnimationName).length;
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
            if (!CheckWeapon()) {
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
            if (IsTargetInvalid) {
                _weaponTimer.CancelLastTimer();
                return;
            }
            if (_weapon == null) {
                this.Logger().Error("Weapon is not setted");
                return;
            }
            _weapon.Fire(_target, _weaponModel, DoDamage);
        }

        private bool CheckWeapon()
        {
            if (_weapon != null) {
                return true;
            }
            this.Logger().Error("Weapon is not setted");
            return false;
        }

        private void DoDamage(GameObject target)
        {
            var damageable = target.RequireComponent<IDamageable>();
            damageable.TakeDamage(_weaponModel.AttackDamage);
            this.Logger().Trace($"Damage applied, target:= {target.name}");
        }

        private void OnDestroy()
        {
            DeleteWeapon();
            if (HasWeaponAnimationHandler) {
                _weaponAnimationHandler.OnFireEvent -= Fire;
            }
        }

        private class ChangeableWeapon
        {
            public BaseWeapon Weapon { get; set; }
            public IWeaponModel Model { get; set; }
            public WeaponTimer Timer { get; set; }
        }
    }
}