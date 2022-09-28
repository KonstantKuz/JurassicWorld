using Dino.Units.Enemy.Config;

namespace Dino.Units.Enemy.Model.EnemyAttack
{
    public class EnemyAttackModel
    {
        public EnemyAttackModel(int level, EnemyUnitConfig unitConfig, EnemyAttacksConfig attacksConfig)
        {
            AttackVariant = unitConfig.AttackVariant;
            var regularDamage = unitConfig.GetDamageForLevel(level, unitConfig.RegularAttackConfig.AttackDamage);
            Regular = new RegularAttackModel(regularDamage, unitConfig.RegularAttackConfig);
            Bulldozing = new BulldozingAttackModel(attacksConfig.BulldozingAttackConfig);
            Jumping = new JumpingAttackModel(attacksConfig.JumpingAttackConfig);
        }
        
        public AttackVariant AttackVariant { get; }
        public RegularAttackModel Regular { get; }
        public BulldozingAttackModel Bulldozing { get; }
        public JumpingAttackModel Jumping { get; }
        public float AttackDistance => Regular.AttackDistance;
        public float AttackDamage => Regular.AttackDamage;
        public float AttackInterval => Regular.AttackInterval;
    }
}