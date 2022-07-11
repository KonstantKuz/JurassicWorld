﻿using System;
using System.Collections;
using Dino.Units.Component.Health;
using Dino.Units.Target;
using Dino.Units.Weapon.Projectiles.Params;
using Logger.Extension;
using ModestTree;
using UnityEngine;

namespace Dino.Units.Weapon.Projectiles
{
    public class Beam : MonoBehaviour
    {
        [SerializeField]
        private float _maxLifeTime;
        [Range(0f, 1f)]
        [SerializeField]
        private float _ratioHitTime;

        protected ITarget Target;
        protected UnitType TargetType;
        protected Action<GameObject> HitCallback;
        protected IProjectileParams ProjectileParams;
        protected Transform Barrel;
        
        private float HitTime => _maxLifeTime * _ratioHitTime;

        public void Launch(ITarget target, IProjectileParams projectileParams, Action<GameObject> hitCallback, Transform barrel)
        {
            Assert.IsNotNull(target);
            ProjectileParams = projectileParams;
            HitCallback = hitCallback;
            Barrel = barrel;
            SetTarget(target); 
            StartCoroutine(Shoot());
        }

        private void SetTarget(ITarget target)
        {
            ClearTarget();
            Target = target;
            TargetType = target.UnitType;
            Target.OnTargetInvalid += ClearTarget;
        }
        private IEnumerator Shoot()
        {
            yield return new WaitForSeconds(HitTime);
            TryHitTarget();
            yield return new WaitForSeconds(Math.Abs(_maxLifeTime - HitTime));
            Destroy();
        }

        private void TryHitTarget()
        {
            if (Target == null) {
                return;
            }
            Hit(Target);
        }

        private void Hit(ITarget target)
        {
            var targetObj = target as MonoBehaviour;
            if (targetObj == null) {
                this.Logger().Warn("Target is not a monobehaviour");
                return;
            }
            if (targetObj.GetComponent<IDamageable>() == null) {
                return;
            }
            TryHit(targetObj.gameObject);
        }
        protected virtual void TryHit(GameObject target)
        {
            HitCallback?.Invoke(target);
        }
        private void Destroy()
        {
            ClearTarget();
            HitCallback = null;
            Destroy(gameObject);
        }
        private void ClearTarget()
        {
            if (Target != null) {
                Target.OnTargetInvalid -= ClearTarget;
            }
            Target = null;
        }
    }
}