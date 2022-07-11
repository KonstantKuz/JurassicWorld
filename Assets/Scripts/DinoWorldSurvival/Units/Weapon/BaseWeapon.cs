using System;
using DinoWorldSurvival.Units.Target;
using DinoWorldSurvival.Units.Weapon.Projectiles.Params;
using JetBrains.Annotations;
using UnityEngine;

namespace DinoWorldSurvival.Units.Weapon
{
    public abstract class BaseWeapon : MonoBehaviour
    {
        public abstract void Fire(ITarget target, [CanBeNull] IProjectileParams projectileParams, Action<GameObject> hitCallback);
    }
}