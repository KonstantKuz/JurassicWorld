using System;
using UniRx;
using UnityEngine;

namespace Dino.Weapon.Components
{
    public class WeaponTimer
    {
        private readonly float _attackInterval;

        private float AttackInterval => Math.Max(_attackInterval, 0);
        private FloatReactiveProperty LastAttackTime { get; }
        public BoolReactiveProperty IsAttackReady { get; }

        private float ReloadingTime => Time.time - LastAttackTime.Value;
        public float ReloadProgress => ReloadingTime / AttackInterval;
        public float ReloadTimeLeft => AttackInterval - ReloadingTime;

        public WeaponTimer(float attackInterval)
        {
            _attackInterval = attackInterval;
            LastAttackTime = new FloatReactiveProperty();
            IsAttackReady = new BoolReactiveProperty();
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