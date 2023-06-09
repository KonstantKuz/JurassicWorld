﻿using System;
using Dino.Units.Component.Target;
using Dino.Weapon.Model;
using JetBrains.Annotations;
using UnityEngine;

namespace Dino.Weapon
{
    public abstract class BaseWeapon : MonoBehaviour
    {
        public abstract void Fire(ITarget target, [CanBeNull] IWeaponModel weaponModel, Action<GameObject> hitCallback);
    }
}