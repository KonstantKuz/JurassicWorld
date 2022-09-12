using System;
using Dino.Inventory.Model;
using Dino.Units.Component.Target;
using Dino.Units.Player.Model;
using JetBrains.Annotations;
using UnityEngine;

namespace Dino.Weapon.Components
{
    public class WeaponWrapper
    {
        public readonly ItemId WeaponId;
        public readonly PlayerWeaponModel Model;
        public readonly WeaponTimer Timer;
        public readonly Clip Clip;
        
        [CanBeNull]
        public BaseWeapon WeaponObject;
        
        public bool IsWeaponReadyToFire => Clip.HasAmmo && Timer.IsAttackReady.Value;

        public WeaponWrapper(ItemId weaponId, PlayerWeaponModel model, WeaponTimer timer, Clip clip)
        {
            WeaponId = weaponId;
            Model = model;
            Timer = timer;
            Clip = clip;
        }
        public static WeaponWrapper Create(ItemId weaponId,
                                           PlayerWeaponModel model,
                                           WeaponTimer timer,
                                           Clip clip)
        {
            return new WeaponWrapper(weaponId, model, timer, clip);
        }

        public void Fire(ITarget target, Action<GameObject> hitCallback)
        {
            if (WeaponObject == null) {
                throw new NullReferenceException("Firing error, weapon is not set");
            }
            WeaponObject.Fire(target, Model, hitCallback);
            Clip.OnFire();
        }
     
    }
}