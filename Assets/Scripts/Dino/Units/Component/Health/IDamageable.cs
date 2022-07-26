using System;

namespace Dino.Units.Component.Health
{
    public interface IDamageable
    { 
        void TakeDamage(DamageParams damageParams);
        event Action OnZeroHealth;
        event Action<DamageParams> OnDamageTaken;
        bool DamageEnabled { get; set; } 
    }
}