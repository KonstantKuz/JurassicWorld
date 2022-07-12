using System;
using Dino.Extension;
using Dino.Units.Component.Health;
using Dino.Units.Player.Model;
using Dino.Units.Weapon;
using Feofun.Components;
using Logger.Extension;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace Dino.Units.Player.Attack
{
    public class CircularSawAttack : MonoBehaviour, IInitializable<IUnit>, IUnitDeathEventReceiver
    {
        [SerializeField] private CircularSawWeapon _circularSawWeapon;
        
        private Unit _ownerUnit;
        private PlayerAttackModel _attackModel;
        private CompositeDisposable _disposable;
        
        public void Init(IUnit unit)
        {
            Dispose();
            _disposable = new CompositeDisposable();
            
            _ownerUnit = unit as Unit;
            if (!(unit.Model.AttackModel is PlayerAttackModel attackModel))
            {
                throw new ArgumentException($"Unit must be a player unit, gameObj:= {gameObject.name}");
            }
            _attackModel = attackModel;
            _attackModel.ShotCount.Subscribe(CreateSaws).AddTo(_disposable);
            UpdateRadius();
        }
        
        private void CreateSaws(int count)
        {
            var projectileParams = _attackModel.CreateProjectileParams();
            var targetType = _ownerUnit.TargetUnitType;
            _circularSawWeapon.Init(targetType, projectileParams, DoDamage);
        }

        private void UpdateRadius()
        {
            var projectileParams = _attackModel.CreateProjectileParams();
            _circularSawWeapon.OnParamsChanged(projectileParams);
        }

        public void OnDeath(DeathCause deathCause)
        {
            Dispose();
        }

        private void DoDamage(GameObject target)
        {
            var damageable = target.RequireComponent<IDamageable>();
            damageable.TakeDamage(_attackModel.AttackDamage);
            this.Logger().Trace($"Damage applied, target:= {target.name}");
        }

        private void Dispose()
        {
            _circularSawWeapon.CleanUp();
            _disposable?.Dispose();
            _disposable = null;
            _ownerUnit = null;
        }
    }
}
