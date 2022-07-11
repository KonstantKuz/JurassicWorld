using System;
using Dino.Units.Model;
using UnityEngine;

namespace Dino.Units.Player.Attack
{
    public class WeaponTimerManager : MonoBehaviour, IWeaponTimerManager
    {
        private WeaponTimer _timer;
        
        public void Subscribe(string weaponId, IAttackModel attackModel, Action onAttackReady)
        {
            if (_timer == null) {
                _timer = new WeaponTimer(attackModel.AttackInterval);
            }
            _timer.OnAttackReady += onAttackReady;
        }
        public void Unsubscribe(string weaponId, Action onAttackReady)
        {
            if (_timer == null) {
                return;
            }
            _timer.OnAttackReady -= onAttackReady;
        }
        private void Update()
        {
            _timer?.OnTick();
        }
    }
}