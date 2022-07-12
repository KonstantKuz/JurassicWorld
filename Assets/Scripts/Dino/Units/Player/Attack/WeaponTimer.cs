using System;
using UnityEngine;

namespace Dino.Units.Player.Attack
{
    public class WeaponTimer
    {
        private readonly float _attackInterval;

        private float _timer;
        public event Action OnAttackReady;
  
        private bool IsAttackReady => _timer >= AttackInterval;
        private float AttackInterval => Math.Max(_attackInterval, 0);
        public WeaponTimer(float attackInterval)
        {
            _attackInterval = attackInterval;
            _timer = _attackInterval;
        }

        public void OnTick()
        {
            _timer += Time.deltaTime;
            if (!IsAttackReady) {
                return;
            }
            OnAttackReady?.Invoke();
            _timer = 0f;
        }
    }
}