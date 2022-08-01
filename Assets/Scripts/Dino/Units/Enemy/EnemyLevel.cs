using Dino.Units.Enemy.Config;
using UnityEngine;

namespace Dino.Units.Enemy
{
    public class EnemyLevel : MonoBehaviour
    {
        [field: SerializeField] public int Level { get; private set; } = EnemyUnitConfig.MIN_LEVEL;
    }
}
