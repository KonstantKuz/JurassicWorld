using Dino.Units.Enemy.Config;

namespace Dino.Units.Enemy.Model.EnemyAttack
{
    public class EnemyAttackModel
    {
        public EnemyAttackModel(int level, EnemyUnitConfig unitConfig, EnemyAttackConfig config)
        {
            var regularDamage = unitConfig.GetDamageForLevel(level, config.RegularAttackConfig.AttackDamage);
            Regular = new RegularAttackModel(regularDamage, config.RegularAttackConfig);
            Bulldozing = new BulldozingAttackModel(config.BulldozingAttackConfig);
            Jumping = new JumpingAttackModel(config.JumpingAttackConfig);
        }
        
        public RegularAttackModel Regular { get; }
        public BulldozingAttackModel Bulldozing { get; }
        public JumpingAttackModel Jumping { get; }
        public float AttackDistance => Regular.AttackDistance;
        public float AttackDamage => Regular.AttackDamage;
        public float AttackInterval => Regular.AttackInterval;
    }
}