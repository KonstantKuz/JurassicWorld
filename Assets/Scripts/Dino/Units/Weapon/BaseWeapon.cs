using System;
using Dino.Units.Target;
using Dino.Units.Weapon.Projectiles.Params;
using JetBrains.Annotations;
using UnityEngine;

namespace Dino.Units.Weapon
{
    public abstract class BaseWeapon : MonoBehaviour
    {
        public abstract void Fire(ITarget target, [CanBeNull] IProjectileParams projectileParams, Action<GameObject> hitCallback);
    }
}