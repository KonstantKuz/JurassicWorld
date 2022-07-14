using System;
using Dino.Units;
using Dino.Units.Component.Health;
using Dino.Units.Model;
using Dino.Units.Target;
using Dino.Weapon.Model;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Assertions;

namespace Dino.Weapon.Projectiles
{
    public abstract class Projectile : MonoBehaviour
    {
        [SerializeField]
        private float _speed;
        
        protected Action<GameObject> HitCallback;
        protected UnitType TargetType;
        protected IWeaponModel Params;

        public float Speed => _speed;
        
        public virtual void Launch(ITarget target, IWeaponModel model, Action<GameObject> hitCallback)
        {
            Assert.IsNotNull(target);
            HitCallback = hitCallback;
            TargetType = target.UnitType;
            Params = model;
        }

        public static bool CanDamageTarget(Collider targetCollider, UnitType type, [CanBeNull] out ITarget target)
        {
            target = null;
            if (!targetCollider.TryGetComponent(out ITarget targetComponent)) {
                return false;
            }
            if (!targetComponent.IsTargetValidAndAlive() || type != targetComponent.UnitType) {
                return false;
            }
            if (!targetCollider.TryGetComponent(out IDamageable damageable)) {
                return false;
            }
            target = targetComponent;
            return true;
        }
        protected virtual void TryHit(GameObject target, Vector3 hitPos, Vector3 collisionNorm)
        {
            HitCallback?.Invoke(target);
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (!CanDamageTarget(other, TargetType, out var target)) {
                return;
            }
            TryHit(other.gameObject, transform.position, -transform.forward);
        }
    }
}