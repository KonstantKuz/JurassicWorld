﻿using Survivors.Units.Model;
using UniRx;

namespace Survivors.Units.Enemy.Model
{
    public class EnemyHealthModel : IHealthModel
    {
        public float StartingMaxHealth { get; }
        public IReadOnlyReactiveProperty<float> MaxHealth { get; }

        public EnemyHealthModel(int maxHealth)
        {
            StartingMaxHealth = maxHealth;
            MaxHealth = new ReactiveProperty<float>(maxHealth);
        }
    }
}