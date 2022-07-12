﻿using Dino.Units.Enemy.Config;
using Dino.Units.Model;
using UniRx;
using UnityEngine;

namespace Dino.Units.Enemy.Model
{
    public class EnemyAttackModel : IAttackModel
    {
        public EnemyAttackModel(EnemyAttackConfig config)
        {
            TargetSearchRadius = Mathf.Infinity;
            AttackDistance = config.AttackDistance;
            AttackDamage = config.AttackDamage;
            AttackInterval = new ReactiveProperty<float>(config.AttackInterval);
        }

        public float TargetSearchRadius { get; }
        public float AttackDistance { get; }
        public float AttackDamage { get; }
        public IReadOnlyReactiveProperty<float> AttackInterval { get; }
    }
}