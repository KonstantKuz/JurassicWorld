﻿using System;
using UnityEngine;

namespace Dino.Units.Player.Attack
{
    public class WeaponTimer
    {
        private readonly float _attackInterval;
        private float _lastAttackTime;
        
        public bool IsAttackReady => Time.time >= _lastAttackTime + AttackInterval;
        private float AttackInterval => Math.Max(_attackInterval, 0);
        public WeaponTimer(float attackInterval)
        {
            _attackInterval = attackInterval;
        }
        public void OnAttack()
        {
            _lastAttackTime = Time.time;
        }
        public void CancelLastTimer()
        {
            _lastAttackTime = Time.time - AttackInterval;
        }
    }
}