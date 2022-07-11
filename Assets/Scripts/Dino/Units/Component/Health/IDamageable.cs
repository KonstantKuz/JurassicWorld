using System;

namespace Dino.Units.Component.Health
{
    public interface IDamageable
    { 
        void TakeDamage(float damage);
        event Action OnZeroHealth;
        event Action OnDamageTaken;
        bool DamageEnabled { get; set; } 
    }
}