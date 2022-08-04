using System;
using UniRx;
using UnityEngine;

namespace Dino.Weapon.Components
{
    public class WeaponTimer
    {
        private readonly float _attackInterval;
        private readonly BoolReactiveProperty _isAttackReady;
        private float _lastAttackTime;
        
        public bool IsAttackReady => Time.time >= _lastAttackTime + AttackInterval;
        public float ReloadingTime => Time.time - _lastAttackTime;
        public float AttackInterval => Math.Max(_attackInterval, 0);

        public BoolReactiveProperty IsAttackReadyReactiveProperty => _isAttackReady;
        public WeaponTimer(float attackInterval)
        {
            _attackInterval = attackInterval;
            _isAttackReady = new BoolReactiveProperty();
            SetAttackAsReady();
        }
        public void OnAttack()
        {
            _lastAttackTime = Time.time;
        }
        public void SetAttackAsReady()
        {
            _lastAttackTime = Time.time - AttackInterval;
        }
        
        public void OnTick()
        {
            if (_isAttackReady.Value != IsAttackReady)
                _isAttackReady.SetValueAndForceNotify(IsAttackReady);
        }
    }
}