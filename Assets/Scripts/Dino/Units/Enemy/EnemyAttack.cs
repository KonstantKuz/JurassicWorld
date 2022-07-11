using Dino.Extension;
using Dino.Units.Component.Health;
using Dino.Units.Enemy.Model;
using Dino.Units.Weapon;
using Feofun.Components;
using Logger.Extension;
using UnityEngine;

namespace Dino.Units.Enemy
{
    [RequireComponent(typeof(EnemyAi))]
    public class EnemyAttack : MonoBehaviour, IInitializable<IUnit>, IUpdatableComponent
    {
        private EnemyAi _enemyAi;
        private BaseWeapon _weapon;
        private EnemyAttackModel _attackModel;

        private float _attackTimer;

        
        public void Init(IUnit unit)
        {
            _attackModel = (EnemyAttackModel) unit.Model.AttackModel;
        }

        private void Awake()
        {
            _enemyAi = gameObject.RequireComponent<EnemyAi>();
            _weapon = gameObject.RequireComponentInChildren<BaseWeapon>();
        }

        public void OnTick()
        {
            UpdateTimer();
            if (CanAttack())
            {
                Attack();
            }
        }

        private void UpdateTimer()
        {
            _attackTimer += Time.deltaTime;
        }

        private bool CanAttack()
        {
            return _enemyAi.CurrentTarget != null 
                   && _enemyAi.DistanceToTarget <= _attackModel.AttackDistance 
                   && _attackTimer >= _attackModel.AttackInterval.Value;
        }
        
        private void Attack()
        {
            _weapon.Fire(_enemyAi.CurrentTarget, null, DoDamage);
            _attackTimer = 0;
        }

        private void DoDamage(GameObject target)
        {
            var damageable = target.RequireComponent<IDamageable>();
            damageable.TakeDamage(_attackModel.AttackDamage);
            this.Logger().Trace($"Damage applied, target:= {target.name}");
        }
    }
}