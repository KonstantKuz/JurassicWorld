using Dino.Units.Enemy.Config;

namespace Dino.Units.Enemy.Model.EnemyAttack
{
    public class RegularAttackModel
    {
        public RegularAttackModel(float attackDamage, RegularAttackConfig config)
        {
            AttackDamage = attackDamage;
            AttackDistance = config.AttackDistance;
            AttackInterval = config.AttackInterval;
        }
        
        public float AttackDamage { get; }
        public float AttackDistance { get; }
        public float AttackInterval { get; }
    }
}