using Dino.Units.Enemy.Config;

namespace Dino.Units.Enemy.Model.EnemyAttack
{
    public class BulldozingAttackModel : AttackVariantModel
    {
        public BulldozingAttackModel(BulldozingAttackConfig config)
        {
            Speed = config.MoveSpeed;
            SafeTime = config.SafeTime;
        }
        
        public float Speed { get; }
        public float SafeTime { get; }
    }
}