using DinoWorldSurvival.Units.Enemy.Config;
using DinoWorldSurvival.Units.Model;
using UniRx;
using UnityEngine;

namespace DinoWorldSurvival.Units.Enemy.Model
{
    public class EnemyAttackModel : IAttackModel
    {
        public EnemyAttackModel(EnemyAttackConfig config)
        {
            TargetSearchRadius = Mathf.Infinity;
            AttackDistance = config.AttackRange;
            AttackDamage = config.AttackDamage;
            AttackInterval = new ReactiveProperty<float>(config.AttackInterval);
        }

        public float TargetSearchRadius { get; }
        public float AttackDistance { get; }
        public float AttackDamage { get; }
        public IReadOnlyReactiveProperty<float> AttackInterval { get; }
    }
}