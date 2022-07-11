﻿using System;
using System.Linq;
using DinoWorldSurvival.Units.Component.Health;
using DinoWorldSurvival.Units.Target;
using DinoWorldSurvival.Units.Weapon.Projectiles.Params;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Assertions;

namespace DinoWorldSurvival.Units.Weapon.Projectiles
{
    public abstract class Projectile : MonoBehaviour
    {
        protected Action<GameObject> HitCallback;
        protected UnitType TargetType;
        protected IProjectileParams Params;
        protected float Speed => Params.Speed;

        public virtual void Launch(ITarget target, IProjectileParams projectileParams, Action<GameObject> hitCallback)
        {
            Assert.IsNotNull(target);
            HitCallback = hitCallback;
            TargetType = target.UnitType;
            Params = projectileParams;
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
            TryHitTargetsInRadius(target.transform.position, Params.DamageRadius, TargetType, target, HitCallback);
        }

        public static void TryHitTargetsInRadius(Vector3 hitPosition,
                                                 float damageRadius,
                                                 UnitType targetType,
                                                 GameObject excludedTarget,
                                                 Action<GameObject> hitCallback)
        {
            var hits = GetHits(hitPosition, damageRadius, targetType);
            foreach (var hit in hits) {
                if (hit.gameObject == excludedTarget) {
                    continue;
                }
                if (hit.TryGetComponent(out IDamageable damageable)) {
                    hitCallback?.Invoke(hit.gameObject);
                }
            }
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

        private void OnTriggerEnter(Collider other)
        {
            if (!CanDamageTarget(other, TargetType, out var target)) {
                return;
            }
            TryHit(other.gameObject, transform.position, -transform.forward);
        }
    }
}