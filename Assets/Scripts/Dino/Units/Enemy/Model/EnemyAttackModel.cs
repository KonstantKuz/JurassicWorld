using Dino.Units.Enemy.Config;

namespace Dino.Units.Enemy.Model
{
    public class EnemyAttackModel
    {
        public EnemyAttackModel(float attackDamage, EnemyAttackConfig config)
        {
            AttackDamage = attackDamage;
            AttackDistance = config.AttackDistance;
            AttackInterval = config.AttackInterval;
        }
        public float AttackDistance { get; }
        public float AttackDamage { get; }
        public float AttackInterval { get; }
    }
}