using Dino.Units.Enemy.Config;

namespace Dino.Units.Enemy.Model.EnemyAttack
{
    public class EnemyAttackModel
    {
        public EnemyAttackModel(int level, EnemyUnitConfig unitConfig)
        {
            AttackVariant = unitConfig.AttackVariant;
            var damage = unitConfig.GetDamageForLevel(level, unitConfig.AttackDamage);
            AttackDamage = damage;
            AttackDistance = unitConfig.AttackDistance;
            AttackInterval = unitConfig.AttackInterval;
        }
        
        public AttackVariant AttackVariant { get; }
        public float AttackDamage { get; }
        public float AttackDistance { get; }
        public float AttackInterval { get; }
    }
}