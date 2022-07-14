using System;
using Dino.Extension;
using Dino.Units.Component.Target;
using Dino.Units.Target;
using Dino.Weapon.Model;
using UnityEngine;

namespace Dino.Weapon.Projectiles
{
    public enum BoomerangState
    {
        MoveToTarget,
        Stop,
        ReturnBack
    }
    
    public class Boomerang : Projectile
    {
        private const float STOPPING_DISTANCE = 0.5f;
        [SerializeField] private float _returnDelay;

        private Action<Boomerang> _destroyCallback;
        private Vector3 _initialTargetPosition;
        private Transform _returnPoint;
        private float _startTime;
        
        private float LifeTime => Time.time - _startTime;
        private bool IsTargetPositionReached => Vector3.Distance(transform.position, TargetPosition) < STOPPING_DISTANCE;
        private Vector3 TargetPosition => GetCurrentState() == BoomerangState.ReturnBack ? _returnPoint.position : _initialTargetPosition;

        public void Launch(Transform returnPoint, ITarget target, IWeaponModel model, Action<GameObject> hitCallback, Action<Boomerang> destroyCallBack)
        {
            base.Launch(target, model, hitCallback);

            _initialTargetPosition = transform.position + transform.forward * Params.AttackDistance;
            _destroyCallback = destroyCallBack;
            _returnPoint = returnPoint;
            _startTime = Time.time;
        }

        private void Update()
        {
            if (GetCurrentState() != BoomerangState.Stop)
            {
                UpdatePosition();
            }
            if (GetCurrentState() == BoomerangState.ReturnBack && IsTargetPositionReached)
            {
                Destroy();
            }
        }

        private BoomerangState GetCurrentState()
        {
            var moveToTargetTime = Params.AttackDistance / Speed;
            var returnBackTime = moveToTargetTime + _returnDelay;
            if (LifeTime < moveToTargetTime)
            {
                return BoomerangState.MoveToTarget;
            }
            return LifeTime < returnBackTime ? BoomerangState.Stop : BoomerangState.ReturnBack;
        }

        private void UpdatePosition()
        {
            var moveDirection = TargetPosition - transform.position;
            transform.rotation = Quaternion.LookRotation(moveDirection.XZ());
            transform.position += transform.forward * Speed * Time.deltaTime;
        }
        
        private void Destroy()
        {
            _destroyCallback?.Invoke(this);
            HitCallback = null;
            Destroy(gameObject);
        }
    }
}