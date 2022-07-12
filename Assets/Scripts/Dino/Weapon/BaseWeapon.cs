using System;
using Dino.Units.Target;
using Dino.Weapon.Projectiles.Params;
using JetBrains.Annotations;
using UnityEngine;

namespace Dino.Weapon
{
    public abstract class BaseWeapon : MonoBehaviour
    {
        public abstract void Fire(ITarget target, [CanBeNull] IProjectileParams projectileParams, Action<GameObject> hitCallback);
    }
}