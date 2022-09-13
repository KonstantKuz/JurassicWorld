using System;
using Dino.Inventory.Model;
using Dino.Units.Component.Target;
using Dino.Units.Player.Model;
using Dino.Weapon;
using Dino.Weapon.Components;
using UnityEngine;

namespace Dino.Units.Player.Component
{
    public class WeaponWrapper
    {
        public Item Item { get; set; }
        public BaseWeapon Weapon { get; set; }
        public PlayerWeaponModel Model { get; set; }
        public WeaponTimer Timer { get; set; }

        public void Fire(ITarget target, Action<GameObject> hitCallback)
        {
            Weapon.Fire(target, Model, hitCallback);
        }

        public static WeaponWrapper Create(Item item, BaseWeapon weapon, PlayerWeaponModel playerWeaponModel, WeaponTimer weaponTimer)
        {
            return new WeaponWrapper {
                    Item = item,
                    Weapon = weapon,
                    Model = playerWeaponModel,
                    Timer = weaponTimer
            };
        }
    }
}