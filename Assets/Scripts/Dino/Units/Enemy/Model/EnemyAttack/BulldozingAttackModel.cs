using Dino.Units.Enemy.Config;

namespace Dino.Units.Enemy.Model.EnemyAttack
{
    public class BulldozingAttackModel : AttackVariantModel
    {
        public BulldozingAttackModel(BulldozingAttackConfig config)
        {
            AimSpeed = config.AimSpeed;
            Speed = config.MoveSpeed;
            SafeTime = config.SafeTime;
        }
        
        public float AimSpeed { get; }
        public float Speed { get; }
        public float SafeTime { get; }
    }
}