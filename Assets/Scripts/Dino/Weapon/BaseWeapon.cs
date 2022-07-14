using System;
using Dino.Units.Model;
using Dino.Units.Target;
using JetBrains.Annotations;
using UnityEngine;

namespace Dino.Weapon
{
    public abstract class BaseWeapon : MonoBehaviour
    {
        public abstract void Fire(ITarget target, [CanBeNull] IWeaponModel weaponModel, Action<GameObject> hitCallback);
    }
}