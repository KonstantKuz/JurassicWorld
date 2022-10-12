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
        [SerializeField] private float _maxLifeTime;
        [SerializeField] private float _hitStuckTime;
        [SerializeField] private KickbackReactionParams _kickbackParams;

        private bool _isStuck;
        
        private float TimeLeft { get; set; }
        
        public override void Launch(ITarget target, IWeaponModel model, Action<GameObject> hitCallback)
        {
            base.Launch(target, model, hitCallback);
            _isStuck = false;
            TimeLeft = _maxLifeTime;
        }
        
        private void Update()
        {
            if (!_isStuck)
            {
                UpdatePosition();
            }

            TimeLeft -= Time.deltaTime;
            if (TimeLeft <= 0) {
                Destroy();
            }
        }

        private void UpdatePosition()
        {
            transform.position += transform.forward * Speed * Time.deltaTime;
        }

        protected override void TryHit(GameObject target, Vector3 hitPos, Vector3 collisionNorm)
        {
            base.TryHit(target, hitPos, collisionNorm);
            KickbackReaction.TryExecuteOn(target, -collisionNorm, _kickbackParams);
            AttachToTarget(target);
            TimeLeft = _hitStuckTime;
        }

        private void AttachToTarget(GameObject target)
        {
            _isStuck = true;
            transform.SetParent(target.transform);
        }

        private void Destroy()
        {
            HitCallback = null;
            Destroy(gameObject);
        }
    }
}