using System;
using System.Collections.Generic;
using Dino.Extension;
using Dino.Location;
using Dino.Units.Player;
using Dino.Units.Player.Component;
using Dino.Units.Player.Model;
using Dino.Weapon.Config;
using Dino.Weapon.Model;
using Feofun.Config;
using Zenject;

namespace Dino.Weapon.Service
{
    public class WeaponService
    {
        private readonly Dictionary<WeaponId, Action<WeaponId>> _specialWeapons;

        [Inject]
        private ConfigCollection<WeaponId, WeaponConfig> _weaponConfigs;

        [Inject]
        private World _world;

        private PlayerUnit Player => _world.GetPlayer();

        public WeaponService()
        {
            _specialWeapons = new Dictionary<WeaponId, Action<WeaponId>>();
        }
        public void Set(WeaponId weaponId)
        {
            if (_specialWeapons.ContainsKey(weaponId)) {
                _specialWeapons[weaponId].Invoke(weaponId);
            } 
            else {
                SetWeapon(weaponId);
            }
        }
        
        public void Remove()
        {
            var attack = Player.PlayerAttack;
            attack.DeleteWeapon();
        }

        private void SetWeapon(WeaponId weaponId)
        {
            var model = CreateModel(weaponId);
            var weapon = Player.ActiveItemOwner.GetWeapon();
            var attack = Player.PlayerAttack;
            attack.SetWeapon(model, weapon);
        }
        private PlayerWeaponModel CreateModel(WeaponId weaponId)
        {
            var config = _weaponConfigs.Get(weaponId);
            return new PlayerWeaponModel(config);
        }
    }
}