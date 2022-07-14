using System;
using Dino.Extension;
using Dino.Units.Component.Health;
using UnityEngine;

namespace Dino.Units.Component.Damageable
{
    public class DamageableChild : MonoBehaviour, IDamageable
    {
        private IDamageable _parentDamageable;
        public event Action OnZeroHealth;
        public event Action OnDamageTaken;
        public bool DamageEnabled { get; set; } = true;
        private IDamageable ParentDamageable
        {
            get
            {
                if (_parentDamageable == null) {
                    _parentDamageable = transform.parent.gameObject.RequireComponentInParent<IDamageable>();
                    _parentDamageable.OnZeroHealth += OnParentZeroHealth;
                }
                return _parentDamageable;
            }
        }
        private void OnParentZeroHealth()
        {
            OnZeroHealth?.Invoke();
        }

        public void TakeDamage(float damage)
        {
            if (!DamageEnabled) {
                return;
            }
            ParentDamageable.TakeDamage(damage);
            OnDamageTaken?.Invoke();
        }

        private void OnDestroy()
        {
            if (_parentDamageable != null)
            {
                _parentDamageable.OnZeroHealth -= OnParentZeroHealth;
            }
        }
    }
}