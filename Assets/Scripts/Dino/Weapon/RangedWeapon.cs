using System;
using Dino.Extension;
using Dino.Location.Service;
using Dino.Units.Component.Target;
using Dino.Weapon.Components;
using Dino.Weapon.Model;
using Dino.Weapon.Projectiles;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace Dino.Weapon
{
    public class RangedWeapon : BaseWeapon
    {
        [SerializeField]
        private Transform _barrel;
        [SerializeField]
        private bool _aimInXZPlane;
        [SerializeField]
        private Projectile _ammo;
        [Inject]
        protected WorldObjectFactory ObjectFactory;
        
        
        protected Vector3 BarrelPos; //Seems that in some cases unity cannot correctly take position inside animation event

        public override void Init(WeaponOwner weaponOwner)
        {
            _barrel = weaponOwner.Barrel;
        }

        public override void Fire(ITarget target, IWeaponModel weaponModel, Action<GameObject> hitCallback)
        {
            Assert.IsNotNull(weaponModel);  
            Assert.IsNotNull(_barrel);
            var rotationToTarget = GetShootRotation(BarrelPos, target.Center.position, _aimInXZPlane);
            FireSingleShot(rotationToTarget, target, weaponModel, hitCallback);
        }

        protected virtual void FireSingleShot(Quaternion rotation, ITarget target, IWeaponModel weaponModel, Action<GameObject> hitCallback)
        {
            var projectile = CreateProjectile();
            projectile.transform.SetPositionAndRotation(BarrelPos, rotation);
            projectile.Launch(target, weaponModel, hitCallback);
        }

        protected static Quaternion GetShootRotation(Vector3 shootPos, Vector3 targetPos, bool aimInXZPlane)
        {
            var shootDirection = GetShootDirection(shootPos, targetPos);
            if (aimInXZPlane) {
                shootDirection = shootDirection.XZ();
            }
            return Quaternion.LookRotation(shootDirection);
        }

        protected static Vector3 GetShootDirection(Vector3 shootPos, Vector3 targetPos)
        {
            var dir = targetPos - shootPos;
            return dir.normalized;
        }

        private Projectile CreateProjectile()
        {
            return ObjectFactory.CreateObject(_ammo.gameObject).GetComponent<Projectile>();
        }
        private void LateUpdate()
        {
            if (_barrel == null) return;
            BarrelPos = _barrel.position;
        }
    }
}