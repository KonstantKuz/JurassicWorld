using Dino.Units.Enemy.Config;

namespace Dino.Units.Enemy.Model
{
    public class EnemyAttackModel
    {
        public EnemyAttackModel(EnemyAttackConfig config)
        {
            AttackDistance = config.AttackDistance;
            AttackDamage = config.AttackDamage;
            AttackInterval = config.AttackInterval;
        }
        public float AttackDistance { get; }
        public float AttackDamage { get; }
        public float AttackInterval { get; }
    }
}