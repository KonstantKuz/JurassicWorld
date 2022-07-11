using System;
using DinoWorldSurvival.Units.Model;

namespace DinoWorldSurvival.Units.Player.Attack
{
    public interface IWeaponTimerManager
    {
        void Subscribe(string weaponId, IAttackModel attackModel, Action onAttackReady);
        void Unsubscribe(string weaponId, Action onAttackReady);
    }
}