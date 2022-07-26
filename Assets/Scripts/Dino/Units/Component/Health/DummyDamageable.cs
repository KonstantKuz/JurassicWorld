using System;
using UnityEngine;

namespace Dino.Units.Component.Health
{
    public class DummyDamageable : MonoBehaviour, IDamageable
    {
        public void TakeDamage(DamageParams damageParams)
        {
        }

        public event Action OnZeroHealth = delegate { };
        public event Action<DamageParams> OnDamageTaken = delegate { };
        public bool DamageEnabled { get; set; }
    }
}