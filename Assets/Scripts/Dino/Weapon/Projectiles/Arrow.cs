using System;
using Dino.Units.Component.Target;
using Dino.Units.Target;
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

        private Vector3 _lastTargetPos;

        private ITarget _target;

        private float TimeLeft { get; set; }

        private float LifeTime => _maxLifeTime - TimeLeft;

        private void Update()
        {
            UpdateTargetPosition();
            UpdatePosition();

            TimeLeft -= Time.deltaTime;
        }

        public override void Launch(ITarget target, IWeaponModel model, Action<GameObject> hitCallback)
        {
            base.Launch(target, model, hitCallback);
            SetTarget(target);
            TimeLeft = _maxLifeTime;
        }

        private void SetTarget(ITarget target)
        {
            Assert.IsNull(_target, "we are currently supporting only one call to SetTarget on launch");
            Assert.IsNotNull(target);
            if (!_followTarget)
            {
                _lastTargetPos = target.Center.position;
                return;
            }

            _target = target;
            _target.OnTargetInvalid += ClearTarget;
        }

        private void UpdateTargetPosition()
        {
            if (!_followTarget) return;
            if (!_target.IsTargetValidAndAlive()) return;
            _lastTargetPos = _target.Center.position;
        }

        private void UpdatePosition()
        {
            transform.position += transform.forward * Speed * Time.deltaTime;

            if (LifeTime >= _initialCourseTime && _followTarget)
            {
                var lookRotation = Quaternion.LookRotation(_lastTargetPos - transform.position);
                transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * _rotationSpeed);
            }
        }

        protected override void TryHit(GameObject target, Vector3 hitPos, Vector3 collisionNorm)
        {
            base.TryHit(target, hitPos, collisionNorm);
            Destroy();
        }
        private void Destroy()
        {
            ClearTarget();
            HitCallback = null;
            Destroy(gameObject);
        }

        private void ClearTarget()
        {
            if (_target != null) _target.OnTargetInvalid -= ClearTarget;
            _target = null;
        }
    }
}