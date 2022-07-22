using System;
using DG.Tweening;
using Dino.Extension;
using Dino.Location.Service;
using Dino.Units.Component.Health;
using Dino.Units.Component.Target;
using Dino.Weapon.Model;
using UnityEngine;
using Zenject;

namespace Dino.Weapon.Projectiles
{
    public class Stone : Projectile
    {
        [SerializeField] private float _heightMin;
        [SerializeField] private float _heightMax;
        [SerializeField] private TrailRenderer _trail;

        private Tween _throwMove;
        
        public override void Launch(ITarget target, IWeaponModel weaponModel, Action<GameObject> hitCallback)
        {
            base.Launch(target, weaponModel, hitCallback);
            var targetPos = target.Root.position;
            var moveTime = GetFlightTime(targetPos);
            var maxHeight = GetMaxHeight(targetPos, weaponModel.AttackDistance);
            _throwMove = transform.DOJump(targetPos, maxHeight, 1, moveTime);
            _throwMove.SetEase(Ease.Linear);
            _throwMove.onComplete = Destroy;
        }

        private float GetFlightTime(Vector3 targetPos)
        {
            var distanceToTarget = Vector3.Distance(transform.position, targetPos);
            return distanceToTarget / Speed;
        }

        private float GetMaxHeight(Vector3 targetPos, float maxDistance)
        {
            var distanceToTarget = Vector3.Distance(transform.position, targetPos);
            return MathLib.Remap(distanceToTarget, 0, maxDistance, _heightMin, _heightMax);
        }

        protected override void TryHit(GameObject target, Vector3 hitPos, Vector3 collisionNorm)
        {
            base.TryHit(target, hitPos, collisionNorm);
            Destroy();
        }

        private void Destroy()
        {
            if (_trail != null)
            {
                _trail.transform.SetParent(null);
                Destroy(_trail.gameObject, _trail.time);
            }

            _throwMove.Kill(true);
            HitCallback = null;
            Destroy(gameObject);
        }
    }
}