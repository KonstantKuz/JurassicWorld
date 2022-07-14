﻿using System;
using UniRx;
using UnityEngine;

namespace Dino.Units.Player.Attack
{
    public class WeaponTimer
    {
        private const float FIRST_ATTACK_DELAY = 0.1f;
        private readonly IReadOnlyReactiveProperty<float> _attackInterval;
        private float _timer;
        
        public event Action OnAttackReady;
        private bool IsAttackReady => _timer >= AttackInterval;
        private float AttackInterval => Math.Max(_attackInterval.Value, 0);
        
        public WeaponTimer(IReadOnlyReactiveProperty<float> attackInterval)
        {
            _attackInterval = attackInterval;
            _timer = attackInterval.Value - FIRST_ATTACK_DELAY;
        }

        public void OnTick()
        {
            _timer += Time.deltaTime;
            if (!IsAttackReady) return;
            OnAttackReady?.Invoke();
            _timer = 0f;
        }
    }
}