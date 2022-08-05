using System;
using System.Timers;
using UniRx;
using UnityEngine;

namespace Dino.Weapon.Components
{
    public class WeaponTimer
    {
        private readonly float _attackInterval;
        private FloatReactiveProperty _lastAttackTime;
        private BoolReactiveProperty _isAttackReady;

        private float AttackInterval => Math.Max(_attackInterval, 0);
        private FloatReactiveProperty LastAttackTime => _lastAttackTime ??= new FloatReactiveProperty();
        public BoolReactiveProperty IsAttackReady => _isAttackReady ??= new BoolReactiveProperty();

        private float ReloadingTime => Time.time - LastAttackTime.Value;
        public float ReloadProgress => ReloadingTime / AttackInterval;
        public float ReloadTimeLeft => AttackInterval - ReloadingTime;

        public WeaponTimer(float attackInterval)
        {
            _attackInterval = attackInterval;
            SetAttackAsReady();
        }
        public void OnAttack()
        {
            LastAttackTime.Value = Time.time;
            IsAttackReady.Value = false;
            Observable.Timer(TimeSpan.FromSeconds(AttackInterval)).Subscribe(it => IsAttackReady.Value = true);
        }
        public void SetAttackAsReady()
        {
            LastAttackTime.Value = Time.time - AttackInterval;
            IsAttackReady.Value = true;
        }
    }
}