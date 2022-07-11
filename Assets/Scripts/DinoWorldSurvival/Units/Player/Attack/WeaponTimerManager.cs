using System;
using System.Collections.Generic;
using DinoWorldSurvival.Units.Model;
using Feofun.Components;
using SuperMaxim.Core.Extensions;
using UnityEngine;

namespace DinoWorldSurvival.Units.Player.Attack
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