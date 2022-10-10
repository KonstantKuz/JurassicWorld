using System;
using System.Linq;
using Dino.Location.Service;
using Dino.Units;
using Dino.Units.Component.Health;
using Dino.Units.Component.Target;
using Dino.Weapon.Model;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace Dino.Weapon.Projectiles
{
    public abstract class Projectile : MonoBehaviour
    {
        [SerializeField]
        private float _speed;
        [SerializeField] 
        private GameObject _hitVfx;
        
        protected Action<GameObject> HitCallback;
        protected UnitType TargetType;
        protected IWeaponModel Params;

        public float Speed => _speed;

        [Inject] private WorldObjectFactory _objectFactory;
        
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
            SpawnHitVfx(hitPos, collisionNorm);
        }

        private void SpawnHitVfx(Vector3 position, Vector3 normal)
        {
            if(_hitVfx == null) return;

            var hitVfx = _objectFactory.CreateObject(_hitVfx).transform;
            hitVfx.position = position;
            hitVfx.up = normal;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (!CanDamageTarget(other, TargetType, out var target)) {
                return;
            }
            TryHit(other.gameObject, transform.position, -transform.forward);
        }
        
        public static Collider[] GetHits(Vector3 position, float damageRadius, UnitType targetType)
        {
            var hits = Physics.OverlapSphere(position, damageRadius);
            return hits.Where(go => {
                    var target = go.GetComponent<ITarget>();
                    return target.IsTargetValidAndAlive() && target.UnitType == targetType;
                })
                .ToArray();
        }
    }
}