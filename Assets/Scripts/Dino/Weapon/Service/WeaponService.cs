using System;
using System.Collections.Generic;
using Dino.Inventory.Model;
using Dino.Location;
using Dino.Units.Player;
using Dino.Units.Player.Component;
using Dino.Units.Player.Model;
using Dino.Weapon.Components;
using Dino.Weapon.Config;
using Dino.Weapon.Model;
using Feofun.Config;
using JetBrains.Annotations;
using Logger.Extension;
using Zenject;

namespace Dino.Weapon.Service
{
    public class WeaponService
    {
        private readonly Dictionary<Item, Action<Item, BaseWeapon>> _specialWeapons;
        private readonly Dictionary<Item, WeaponTimer> _weaponTimers;

        [Inject]
        private StringKeyedConfigCollection<WeaponConfig> _weaponConfigs;
        [Inject]
        private World _world;
        
        private PlayerUnit Player => _world.RequirePlayer();
        
        public WeaponService()
        {
            _specialWeapons = new Dictionary<Item, Action<Item, BaseWeapon>>();
            _weaponTimers = new Dictionary<Item, WeaponTimer>();
        }
        public void TrySetWeapon(Item item, BaseWeapon weapon)
        {
            if (IsWeapon(item)) {
                Set(item, weapon);
            } else {
                this.Logger().Debug($"Inventory item:= {item} is not Weapon");
            }
        }
        public void Set(Item item, BaseWeapon weapon)
        {
            if (_specialWeapons.ContainsKey(item)) {
                _specialWeapons[item].Invoke(item, weapon);
            } else {
                SetWeapon(item, weapon);
            }
        }
        public void Remove()
        {
            Player.PlayerAttack.DeleteWeapon();
        }
        private bool IsWeapon(Item item)
        {
            return _weaponConfigs.Contains(item.Id.FullName);
        }

        [CanBeNull]
        public WeaponTimer GetTimer(Item weapon)
        {
            var model = CreateModel(weapon);
            return _weaponTimers.ContainsKey(weapon) ? _weaponTimers[weapon] : CreateTimer(weapon, model);
        }
        
        private void SetWeapon(Item item, BaseWeapon weapon)
        {
            var model = CreateModel(item);
            var attack = Player.PlayerAttack;
            var weaponWrapper = WeaponWrapper.Create(item, weapon, model, GetTimer(item));
            attack.SetWeapon(weaponWrapper);
        }

        private WeaponTimer CreateTimer(Item weapon, IWeaponModel weaponModel)
        {
            _weaponTimers[weapon] = new WeaponTimer(weaponModel.AttackInterval);
            return _weaponTimers[weapon];
        }

        private PlayerWeaponModel CreateModel(Item weapon)
        {
            var config = _weaponConfigs.Get(weapon.Id.FullName);
            return new PlayerWeaponModel(config);
        }
    }
}