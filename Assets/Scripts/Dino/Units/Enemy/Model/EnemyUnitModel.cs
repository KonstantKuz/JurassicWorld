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
        public EnemyAttackModel AttackModel { get; }
        public PatrolStateModel PatrolStateModel { get; }
        public LookAroundStateModel LookAroundStateModel { get; }

        public EnemyUnitModel(EnemyUnitConfig config)
        {
            Id = config.Id;
            MoveSpeed = config.MoveSpeed;
            HealthModel = new EnemyHealthModel(config.Health);
            AttackModel = new EnemyAttackModel(config.EnemyAttackConfig);
            PatrolStateModel = new PatrolStateModel(config.PatrolStateConfig);
            LookAroundStateModel = new LookAroundStateModel(config.LookAroundStateConfig);
        }
    }
}
