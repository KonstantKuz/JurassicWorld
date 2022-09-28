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

        public EnemyUnitModel(EnemyUnitConfig unitConfig, int level)
        {
            Id = unitConfig.Id;
            Level = level;
            MoveSpeed = unitConfig.MoveSpeed;
            HealthModel = new EnemyHealthModel(unitConfig.GetHealthForLevel(Level));
            AttackModel = new EnemyAttackModel(level, unitConfig);
            PatrolStateModel = new PatrolStateModel(unitConfig.PatrolStateConfig);
            LookAroundStateModel = new LookAroundStateModel(unitConfig.LookAroundStateConfig);
        }
    }
}
