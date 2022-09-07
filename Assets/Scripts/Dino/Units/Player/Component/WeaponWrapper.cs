using System;
using Dino.Inventory.Model;
using Dino.Inventory.Service;
using Dino.Units.Component.Target;
using Dino.Units.Player.Model;
using Dino.Weapon;
using Dino.Weapon.Components;
using Logger.Extension;
using UniRx;
using UnityEngine;

namespace Dino.Units.Player.Component
{
    public class Clip
    {
        public readonly ReactiveProperty<int> ChargeCountProperty = new ReactiveProperty<int>(0);
        
        public int ChargeCount
        {
            get => ChargeCountProperty.Value;
            set
            {
                if (value < 0) {
                    this.Logger().Error("ChargeCount cannot be negative");
                    return;
                }
                ChargeCountProperty.SetValueAndForceNotify(value);
                
            }
        }
        public bool HasCharge => ChargeCount > 0;
        
        public Clip(InventoryService inventoryService, PlayerWeaponModel playerWeaponModel)
        {
            inventoryService.DecreaseItems();
            inventoryService.InventoryProperty.Subscribe(it => {


            });
        }
        public void UpdateChargeCount()
        {
            
        }
    }

    public class WeaponWrapper
    {
        public ItemId WeaponId { get; }
        public BaseWeapon Weapon { get; }
        public PlayerWeaponModel Model { get; }
        public WeaponTimer Timer { get; }
        public Clip Clip { get; }

        public void Fire(ITarget target, Action<GameObject> hitCallback)
        {
            Weapon.Fire(target, Model, hitCallback);
            Clip.ChargeCount--;
        }

        public static WeaponWrapper Create(ItemId weaponId, BaseWeapon weapon, PlayerWeaponModel playerWeaponModel, WeaponTimer weaponTimer)
        {
            return new WeaponWrapper {
                    WeaponId = weaponId,
                    Weapon = weapon,
                    Model = playerWeaponModel,
                    Timer = weaponTimer
            };
        }
    }
}