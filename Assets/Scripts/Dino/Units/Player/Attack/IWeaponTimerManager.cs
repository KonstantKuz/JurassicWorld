using System;
using Dino.Units.Model;

namespace Dino.Units.Player.Attack
{
    public interface IWeaponTimerManager
    {
        void Subscribe(string weaponId, IWeapon weapon, Action onAttackReady);
        void Unsubscribe(string weaponId, Action onAttackReady);
    }
}