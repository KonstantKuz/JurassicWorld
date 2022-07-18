using System;
using System.Collections.Generic;
using Dino.Location;
using Dino.Units.Player;
using Dino.Units.Player.Model;
using Dino.Weapon.Config;
using Dino.Weapon.Model;
using Feofun.Config;
using Zenject;

namespace Dino.Weapon.Service
{
    public class WeaponService
    {
        private readonly Dictionary<WeaponId, Action<WeaponId, BaseWeapon>> _specialWeapons;

        [Inject]
        private ConfigCollection<WeaponId, WeaponConfig> _weaponConfigs;

        [Inject]
        private World _world;

        private PlayerUnit Player => _world.GetPlayer();

        public WeaponService()
        {
            _specialWeapons = new Dictionary<WeaponId, Action<WeaponId, BaseWeapon>>();
        }
        public void Set(WeaponId weaponId, BaseWeapon baseWeapon)
        {
            if (_specialWeapons.ContainsKey(weaponId)) {
                _specialWeapons[weaponId].Invoke(weaponId, baseWeapon);
            } 
            else {
                SetWeapon(weaponId, baseWeapon);
            }
        }
        
        public void Remove()
        { 
            Player.PlayerAttack.DeleteWeapon();
        }

        private void SetWeapon(WeaponId weaponId, BaseWeapon baseWeapon)
        {
            var model = CreateModel(weaponId);
            var attack = Player.PlayerAttack;
            attack.SetWeapon(model, baseWeapon);
        }
        private PlayerWeaponModel CreateModel(WeaponId weaponId)
        {
            var config = _weaponConfigs.Get(weaponId);
            return new PlayerWeaponModel(config);
        }
    }
}