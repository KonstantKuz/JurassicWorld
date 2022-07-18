using System;
using System.Collections.Generic;
using Dino.Location;
using Dino.Units.Player;
using Dino.Units.Player.Model;
using Dino.Weapon.Config;
using Dino.Weapon.Model;
using Feofun.Config;
using Logger.Extension;
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
        public void TrySetWeapon(string itemId, BaseWeapon weapon)
        {
            if (IsWeapon(itemId, out var weaponId)) {
                Set(weaponId, weapon);
            } else {
                this.Logger().Debug($"Inventory item:= {itemId} is not Weapon");
            }
        }
        public void Set(WeaponId weaponId, BaseWeapon weapon)
        {
            if (_specialWeapons.ContainsKey(weaponId)) {
                _specialWeapons[weaponId].Invoke(weaponId, weapon);
            } else {
                SetWeapon(weaponId, weapon);
            }
        }
        public void Remove()
        {
            Player.PlayerAttack.DeleteWeapon();
        }
        private bool IsWeapon(string itemId, out WeaponId weaponId)
        {
            return Enum.TryParse(itemId, out weaponId);
        }
        private void SetWeapon(WeaponId weaponId, BaseWeapon weapon)
        {
            var model = CreateModel(weaponId);
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