using UnityEngine;

namespace Dino.Units.Enemy.Config
{
    public class EnemyAttackConfig : ScriptableObject
    {
        [field: SerializeField] public AttackVariant AttackVariant { get; private set; }
    }
}