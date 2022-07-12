﻿using System;
using Dino.Location.Service;
using Dino.Units.Target;
using Dino.Weapon.Projectiles.Params;
using UnityEngine;
using Zenject;

namespace Dino.Weapon.Projectiles
{
    [RequireComponent(typeof(Rigidbody))]
    public class Bullet : Projectile
    {
        [SerializeField]
        private float _maxLifeTime;
        [SerializeField]
        private GameObject _hitVfx;

        [Inject]
        private WorldObjectFactory _objectFactory;
        
        private float _timeLeft;

        public override void Launch(ITarget target, IProjectileParams projectileParams, Action<GameObject> hitCallback)
        {
            base.Launch(target, projectileParams, hitCallback);
            SetupBullet();
        }

        private void SetupBullet()
        {
            _timeLeft = _maxLifeTime;
        }
        protected override void TryHit(GameObject target, Vector3 hitPos, Vector3 collisionNorm)
        {
            base.TryHit(target, hitPos, collisionNorm);
            PlayVfx(hitPos, collisionNorm);
            Destroy();
        }

        private void Update()
        {
            _timeLeft -= Time.deltaTime;
            UpdatePosition();
            if (_timeLeft > 0) {
                return;
            }
            
            Destroy();
        }
        private void UpdatePosition()
        {
            transform.position += transform.forward * Speed * Time.deltaTime;
        }
        private void Destroy()
        {
            gameObject.SetActive(false);
            HitCallback = null;
            Destroy(gameObject);
        }

        protected void PlayVfx(Vector3 pos, Vector3 up)
        {
            if (_hitVfx == null) return;
            var vfx = _objectFactory.CreateObject(_hitVfx);
            vfx.transform.SetPositionAndRotation(pos, Quaternion.LookRotation(up));
        }
    }
}