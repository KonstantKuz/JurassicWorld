using Dino.Units.Enemy.Config;
using Dino.Units.Enemy.Model.EnemyAttack;
using Dino.Units.Model;

namespace Dino.Units.Enemy.Model
{
    public class EnemyUnitModel : IUnitModel
    {
        public string Id { get; }
        public int Level { get; }
        public float MoveSpeed { get; }
        public IHealthModel HealthModel { get; }
        public EnemyAttackModel AttackModel { get; }
        public PatrolStateModel PatrolStateModel { get; }
        public LookAroundStateModel LookAroundStateModel { get; }

        public EnemyUnitModel(EnemyUnitConfig config, int level)
        {
            Id = config.Id;
            Level = level;
            MoveSpeed = config.MoveSpeed;
            HealthModel = new EnemyHealthModel(config.GetHealthForLevel(Level));
            AttackModel = new EnemyAttackModel(level, config, config.EnemyAttackConfig);
            PatrolStateModel = new PatrolStateModel(config.PatrolStateConfig);
            LookAroundStateModel = new LookAroundStateModel(config.LookAroundStateConfig);
        }
    }
}
