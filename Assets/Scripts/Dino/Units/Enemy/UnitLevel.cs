using Dino.Units.Component;
using Dino.Units.Enemy.Config;
using UnityEngine;

namespace Dino.Units.Enemy
{
    public class UnitLevel : MonoBehaviour, ILevelStatOwner
    {
        [field: SerializeField] public int Level { get; private set; } = EnemyUnitConfig.MIN_LEVEL;
    }
}
