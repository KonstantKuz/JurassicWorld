using System;
using Dino.Units.Model;
using Feofun.Components;
using UnityEngine;

namespace Dino.Units.Player.Attack
{
    public class WeaponTimerManager : MonoBehaviour, IUpdatableComponent, IWeaponTimerManager
    {
        private WeaponTimer _timer;
        
        public void Subscribe(string weaponId, IAttackModel attackModel, Action onAttackReady)
        {
            if (_timer == null) {
                _timer = new WeaponTimer(attackModel.AttackInterval);
            }
            _timer.Init();
            _timer.OnAttackReady += onAttackReady;
        }
        public void Unsubscribe(string weaponId, Action onAttackReady)
        {
            if (_timer == null) {
                return;
            }
            _timer.OnAttackReady -= onAttackReady;
        }
        public void OnTick()
        {
            _timer?.OnTick();
        }
    }
}