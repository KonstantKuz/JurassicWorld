using System;
using Dino.Units.Component.DamageReaction;
using Dino.Units.Component.Target;
using Dino.Weapon.Model;
using UnityEngine;
using UnityEngine.Assertions;

namespace Dino.Weapon.Projectiles
{
    public sealed class Arrow : Projectile
    {
        [SerializeField] private bool _followTarget;
        [SerializeField] private float _rotationSpeed;
        [SerializeField] private float _maxLifeTime;
        [SerializeField] private float _initialCourseTime;
        [SerializeField] private float _hitStuckTime;
        [SerializeField] private KickbackReactionParams _kickbackParams;

        private bool _isAttached;

        private ITarget _target;

        private float TimeLeft { get; set; }

        private float LifeTime => _maxLifeTime - TimeLeft;
        
        public override void Launch(ITarget target, IWeaponModel model, Action<GameObject> hitCallback)
        {
            base.Launch(target, model, hitCallback);
            _isAttached = false;
            SetTarget(target);
            TimeLeft = _maxLifeTime;
        }
        private void Update()
        {
            if (!_isAttached)
            {
                UpdatePosition();
            }

            TimeLeft -= Time.deltaTime;
            if (TimeLeft <= 0) {
                Destroy();
            }
        }

        private void SetTarget(ITarget target)
        {
            Assert.IsNull(_target, "we are currently supporting only one call to SetTarget on launch");
            Assert.IsNotNull(target);
            _target = target;
            _target.OnTargetInvalid += ClearTarget;
        }

        private void UpdatePosition()
        {
            transform.position += transform.forward * Speed * Time.deltaTime;

            if (!_followTarget || !_target.IsTargetValidAndAlive())
            {
                return;
            }
            
            if (LifeTime >= _initialCourseTime)
            {
                var lookRotation = Quaternion.LookRotation(_target.Center.position - transform.position);
                transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * _rotationSpeed);
            }
        }

        protected override void TryHit(GameObject target, Vector3 hitPos, Vector3 collisionNorm)
        {
            base.TryHit(target, hitPos, collisionNorm);
            KickbackReaction.TryExecuteOn(target, -collisionNorm, _kickbackParams);
            AttachToTarget(target);
            ClearTarget();
            TimeLeft = _hitStuckTime;
        }

        private void AttachToTarget(GameObject target)
        {
            _isAttached = true;
            transform.SetParent(target.transform);
        }

        private void Destroy()
        {
            ClearTarget();
            HitCallback = null;
            Destroy(gameObject);
        }

        private void ClearTarget()
        {
            if (_target != null) {
                _target.OnTargetInvalid -= ClearTarget;
                _target = null;
            }
        }
    }
}