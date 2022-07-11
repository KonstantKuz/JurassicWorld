using System;
using Dino.Units.Model;

namespace Dino.Units.Player.Attack
{
    public interface IWeaponTimerManager
    {
        void Subscribe(string weaponId, IAttackModel attackModel, Action onAttackReady);
        void Unsubscribe(string weaponId, Action onAttackReady);
    }
}