using Dino.Units.Enemy.Config;

namespace Dino.Units.Enemy.Model.EnemyAttack
{
    public class JumpingAttackModel
    {
        public JumpingAttackModel(JumpingAttackConfig config)
        {
            AimSpeed = config.AimSpeed;
            Duration = config.Duration;
            Height = config.Height;
            DamageRadius = config.DamageRadius;
            SafeTime = config.SafeTime;
        }
        
        public float AimSpeed { get; }
        public float Duration { get; }
        public float Height { get; }
        public float DamageRadius { get; }
        public float SafeTime { get; }
    }
}