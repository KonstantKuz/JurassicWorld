using DinoWorldSurvival.Extension;
using DinoWorldSurvival.Units.Component.Health;
using DinoWorldSurvival.Units.Player.Model;
using DinoWorldSurvival.Units.Weapon;
using Feofun.Components;
using Logger.Extension;
using UnityEngine;

namespace DinoWorldSurvival.Units.Player.Attack
{
    public class IceWaveAttack : MonoBehaviour, IInitializable<IUnit>
    {
        [SerializeField] private IceWaveWeapon _iceWaveWeapon;

        private Unit _owner;
        private IWeaponTimerManager _weaponTimer;
        private PlayerAttackModel _playerAttackModel;
        
        public void Init(IUnit unit)
        {
            _owner = (Unit) unit;
            _playerAttackModel = (PlayerAttackModel) unit.Model.AttackModel;
            _weaponTimer = _owner.GameObject.RequireComponentInChildren<IWeaponTimerManager>();
            _weaponTimer.Subscribe(_owner.ObjectId, _playerAttackModel, OnAttackReady);
        }

        private void OnAttackReady()
        {
            var parent = _owner.transform;
            var projectileParams = _playerAttackModel.CreateProjectileParams();
            var targetType = _owner.TargetUnitType;
            _iceWaveWeapon.Fire(parent, targetType, projectileParams, DoDamage);
        }

        private void DoDamage(GameObject target)
        {
            var damageable = target.RequireComponent<IDamageable>();
            damageable.TakeDamage(_playerAttackModel.AttackDamage);
            this.Logger().Trace($"Damage applied, target:= {target.name}");
        }

        private void OnDestroy()
        {
            _weaponTimer.Unsubscribe(_owner.ObjectId, OnAttackReady);
        }
    }
}
