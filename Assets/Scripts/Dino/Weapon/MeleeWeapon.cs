using System;
using Dino.Units.Component.Health;
using Dino.Units.Component.Target;
using Dino.Weapon.Components;
using Dino.Weapon.Model;
using Logger.Extension;
using UnityEngine;

namespace Dino.Weapon
{
    public class MeleeWeapon : BaseWeapon
    {
        public override void Init(WeaponOwner weaponOwner)
        {
            throw new NotImplementedException();
        }

        public override void Fire(ITarget target, IWeaponModel weaponModel, Action<GameObject> hitCallback)
        {
            var targetObj = target as MonoBehaviour;
            if (targetObj == null) {
                this.Logger().Warn("Target is not a monobehaviour");
                return;
            }
            if (targetObj.GetComponent<IDamageable>() == null) {
                this.Logger().Warn("Target has no damageable component");
                return;
            }
            hitCallback?.Invoke(targetObj.gameObject);
        }
    }
}