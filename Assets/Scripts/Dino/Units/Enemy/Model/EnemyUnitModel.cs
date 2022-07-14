using Dino.Units.Enemy.Config;
using Dino.Units.Model;
using UnityEngine;
using UnityEngine.Assertions;

namespace Dino.Units.Enemy.Model
{
    public class EnemyUnitModel : IUnitModel
    {
        public string Id { get; }
        public float MoveSpeed { get; }
        public IHealthModel HealthModel { get; }
        public IAttackModel AttackModel { get; }
        public EnemyBehaviourModel EnemyBehaviourModel { get; }
        
        public EnemyUnitModel(EnemyUnitConfig config)
        {
            Id = config.Id;
            MoveSpeed = config.MoveSpeed;
            HealthModel = new EnemyHealthModel(config.Health);
            AttackModel = new EnemyAttackModel(config.EnemyAttackConfig);
            EnemyBehaviourModel = new EnemyBehaviourModel(config.EnemyBehaviourConfig);
        }
    }
}