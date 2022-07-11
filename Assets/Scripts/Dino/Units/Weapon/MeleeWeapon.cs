using System;
using Dino.Units.Component.Health;
using Dino.Units.Target;
using Dino.Units.Weapon.Projectiles.Params;
using Logger.Extension;
using UnityEngine;

namespace Dino.Units.Weapon
{
    public class MeleeWeapon : BaseWeapon
    {
        
        public override void Fire(ITarget target, IProjectileParams chargeParams, Action<GameObject> hitCallback)
        {
            var targetObj = target as MonoBehaviour;
            if (targetObj == null)
            {
                this.Logger().Warn("Target is not a monobehaviour");
                return;
            }

            if (targetObj.GetComponent<IDamageable>() == null)
            {
                this.Logger().Warn("Target has no damageable component");
                return;
            }
            hitCallback?.Invoke(targetObj.gameObject);
        }
    }
}