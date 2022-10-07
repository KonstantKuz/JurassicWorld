using System;

namespace Dino.Units.Component.Health
{
    public interface IDamageable
    { 
        void TakeDamage(HitParams hitParams);
        event Action OnZeroHealth;
        event Action<HitParams> OnDamageTaken;
        bool DamageEnabled { get; set; } 
        bool IsUnAware { get; }
    }
}