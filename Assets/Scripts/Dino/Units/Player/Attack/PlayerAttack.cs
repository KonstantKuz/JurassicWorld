using System.Linq;
using Dino.Extension;
using Dino.Units.Component.Health;
using Dino.Units.Component.TargetSearcher;
using Dino.Units.Player.Model;
using Dino.Units.Player.Movement;
using Dino.Units.Target;
using Dino.Units.Weapon;
using Feofun.Components;
using JetBrains.Annotations;
using Logger.Extension;
using UniRx;
using UnityEngine;

namespace Dino.Units.Player.Attack
{
    [RequireComponent(typeof(ITargetSearcher))]
    [RequireComponent(typeof(MovementController))]
    public class PlayerAttack : MonoBehaviour, IUpdatableComponent, IInitializable<IUnit>
    {
        private static readonly int AttackSpeedMultiplierHash = Animator.StringToHash("AttackSpeedMultiplier");
        private static readonly int AttackHash = Animator.StringToHash("Attack");
        
        [SerializeField]
        private bool _rotateToTarget = true;
        [SerializeField]
        private string _attackAnimationName;
        
        private BaseWeapon _weapon;
        private PlayerAttackModel _playerAttackModel;
        private Animator _animator;
        private ITargetSearcher _targetSearcher;
        private MovementController _movementController;
        private Unit _owner;
        private IWeaponTimerManager _timerManager;
        private CompositeDisposable _disposable;
        
        
        [CanBeNull]
        private WeaponAnimationHandler _weaponAnimationHandler;
        [CanBeNull]
        private ITarget _target;
        
        private bool IsTargetInvalid => !_target.IsTargetValidAndAlive();
        private bool HasWeaponAnimationHandler => _weaponAnimationHandler != null;
        public void Init(IUnit unit)
        {
            Dispose();
            _disposable = new CompositeDisposable();
            _owner = (Unit) unit;
            _playerAttackModel = (PlayerAttackModel) unit.Model.AttackModel;
            
            _playerAttackModel.AttackInterval.Subscribe(UpdateAnimationSpeed).AddTo(_disposable);
            if (HasWeaponAnimationHandler) {
                _weaponAnimationHandler.OnFireEvent += Fire;
            }
            InitWeaponTimer();
        }

        private void InitWeaponTimer()
        {
            _timerManager = _weapon.GetComponent<IWeaponTimerManager>() ?? _owner.GetComponent<IWeaponTimerManager>();
            _timerManager.Subscribe(_owner.ObjectId, _playerAttackModel, OnAttackReady);
        }
        
        private void Awake()
        {
            _weapon = gameObject.RequireComponentInChildren<BaseWeapon>();
            _animator = gameObject.RequireComponentInChildren<Animator>();
            _targetSearcher = GetComponent<ITargetSearcher>();
            _movementController = GetComponent<MovementController>();

            _weaponAnimationHandler = GetComponentInChildren<WeaponAnimationHandler>();
        }
        private void OnAttackReady()
        {
            if (CanAttack(_target)) {
                Attack();
            }
        }

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
            _target = FindTarget();
            if (_rotateToTarget) {
                _movementController.RotateToTarget(_target?.Center);
            }
        }
        private bool CanAttack([CanBeNull] ITarget target) => target != null;

        private void Attack()
        {
            _animator.SetTrigger(AttackHash);
            if (!HasWeaponAnimationHandler) {
                Fire();
            }
        }

        private void Fire()
        {
            if (IsTargetInvalid) {
                return;
            }
            _weapon.Fire(_target, _playerAttackModel.CreateProjectileParams(), DoDamage);
        }

        private void DoDamage(GameObject target)
        {
            var damageable = target.RequireComponent<IDamageable>();
            damageable.TakeDamage(_playerAttackModel.AttackDamage);
            this.Logger().Trace($"Damage applied, target:= {target.name}");
        }

        private void OnDestroy()
        {
            Dispose();
            if (HasWeaponAnimationHandler) {
                _weaponAnimationHandler.OnFireEvent -= Fire;
            }
            _timerManager.Unsubscribe(_owner.ObjectId, OnAttackReady);
        }
        private void Dispose()
        {
            _disposable?.Dispose();
            _disposable = null;
        }
    }
}