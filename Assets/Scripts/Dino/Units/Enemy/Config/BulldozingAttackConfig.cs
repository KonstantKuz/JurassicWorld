using UnityEngine;

namespace Dino.Units.Enemy.Config
{
    [CreateAssetMenu(menuName = "EnemyAttackConfig/BulldozingAttackConfig", fileName = "BulldozingAttackConfig")]
    public class BulldozingAttackConfig : EnemyAttackConfig
    {
        public float MoveSpeed;
        public float SafeTime;
    }
}