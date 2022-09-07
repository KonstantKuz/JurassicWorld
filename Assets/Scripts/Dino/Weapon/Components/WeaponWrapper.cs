﻿using System;
using Dino.Units.Component.Target;
using Dino.Units.Player.Model;
using JetBrains.Annotations;
using UnityEngine;

namespace Dino.Weapon.Components
{
    public class WeaponWrapper : IDisposable
    {
        public string WeaponId { get; set; }
        [CanBeNull]
        public BaseWeapon Weapon { get; set; }
        public PlayerWeaponModel Model { get; set; }
        public WeaponTimer Timer { get; set; }
        public Clip Clip { get; set; }

        public void Fire(ITarget target, Action<GameObject> hitCallback)
        {
            if (Weapon == null) {
                throw new NullReferenceException("Firing error, weapon is not set");
            }
            Weapon.Fire(target, Model, hitCallback);
            Clip.OnFire();
        }
        public static WeaponWrapper Create(string weaponId,
                                           PlayerWeaponModel playerWeaponModel,
                                           WeaponTimer weaponTimer,
                                           Clip clip)
        {
            return new WeaponWrapper {
                    WeaponId = weaponId,
                    Model = playerWeaponModel,
                    Timer = weaponTimer,
                    Clip = clip,
            };
        }

        public void Dispose()
        {
            Clip?.Dispose();
        }
    }
}