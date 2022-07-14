using System;
using Dino.Extension;
using Dino.Location.Service;
using Dino.Units.Component.Target;
using Dino.Units.Model;
using Dino.Units.Target;
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
        [SerializeField]
        private float _angleBetweenShots;
        [Inject]
        protected WorldObjectFactory ObjectFactory;

        protected Transform Barrel => _barrel;
        
        protected Vector3 BarrelPos; //Seems that in some cases unity cannot correctly take position inside animation event
        
        protected bool AimInXZPlane => _aimInXZPlane;
        protected Projectile Ammo => _ammo;
        protected float AngleBetweenShots => _angleBetweenShots;

        public override void Fire(ITarget target, IWeaponModel weaponModel, Action<GameObject> hitCallback)
        {
            Assert.IsNotNull(weaponModel);
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
            BarrelPos = _barrel.position;
        }
    }
}