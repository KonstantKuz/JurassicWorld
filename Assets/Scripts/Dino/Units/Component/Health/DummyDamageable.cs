using System;
using UnityEngine;

namespace Dino.Units.Component.Health
{
    public class DummyDamageable : MonoBehaviour, IDamageable
    {
        public void TakeDamage(HitParams hitParams)
        {
        }

        public event Action OnZeroHealth = delegate { };
        public event Action<HitParams> OnDamageTaken = delegate { };
        public bool DamageEnabled { get; set; }
    }
}