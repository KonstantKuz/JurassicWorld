using Dino.Units.Enemy.Config;

namespace Dino.Units.Enemy.Model.EnemyAttack
{
    public class BulldozingAttackModel
    {
        public BulldozingAttackModel(BulldozingAttackConfig config)
        {
            RotationSpeed = config.RotationSpeed;
            Speed = config.Speed;
            SafeTime = config.SafeTime;
        }
        
        public float RotationSpeed { get; }
        public float Speed { get; }
        public float SafeTime { get; }
    }
}