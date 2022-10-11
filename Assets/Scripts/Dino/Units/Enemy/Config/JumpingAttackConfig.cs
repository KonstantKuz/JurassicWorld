using UnityEngine;

namespace Dino.Units.Enemy.Config
{
    [CreateAssetMenu(menuName = "EnemyAttackConfig/JumpingAttackConfig", fileName = "JumpingAttackConfig")]
    public class JumpingAttackConfig : EnemyAttackConfig
    {
        public float Duration;
        public float Height;
        public float DamageRadius;
        public float SafeTime;
    }
}