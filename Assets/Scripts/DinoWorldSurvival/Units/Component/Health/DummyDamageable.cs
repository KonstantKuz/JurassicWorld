using System;
using UnityEngine;

namespace DinoWorldSurvival.Units.Component.Health
{
    public class DummyDamageable : MonoBehaviour, IDamageable
    {
        public void TakeDamage(float damage)
        {
        }

        public event Action OnZeroHealth = delegate { };
        public event Action OnDamageTaken = delegate { };
        public bool DamageEnabled { get; set; }
    }
}