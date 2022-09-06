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
        private readonly Dictionary<ItemId, Action<ItemId, BaseWeapon>> _specialWeapons;
        private readonly Dictionary<ItemId, WeaponTimer> _weaponTimers;

        [Inject]
        private StringKeyedConfigCollection<WeaponConfig> _weaponConfigs;
        [Inject]
        private World _world;
        
        private PlayerUnit Player => _world.Player;
        
        public WeaponService()
        {
            _specialWeapons = new Dictionary<ItemId, Action<ItemId, BaseWeapon>>();
            _weaponTimers = new Dictionary<ItemId, WeaponTimer>();
        }
        public void TrySetWeapon(ItemId itemId, BaseWeapon weapon)
        {
            if (IsWeapon(itemId)) {
                Set(itemId, weapon);
            } else {
                this.Logger().Debug($"Inventory item:= {itemId} is not Weapon");
            }
        }
        public void Set(ItemId weaponId, BaseWeapon weapon)
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
        private bool IsWeapon(ItemId itemId)
        {
            return _weaponConfigs.Contains(itemId.FullName);
        }

        [CanBeNull]
        public WeaponTimer GetTimer(ItemId weaponId)
        {
            var model = CreateModel(weaponId);
            return _weaponTimers.ContainsKey(weaponId) ? _weaponTimers[weaponId] : CreateTimer(weaponId, model);
        }
        
        private void SetWeapon(ItemId weaponId, BaseWeapon weapon)
        {
            var model = CreateModel(weaponId);
            var attack = Player.PlayerAttack;
            var weaponWrapper = WeaponWrapper.Create(weaponId, weapon, model, GetTimer(weaponId));
            attack.SetWeapon(weaponWrapper);
        }

        private WeaponTimer CreateTimer(ItemId weaponId, IWeaponModel weaponModel)
        {
            _weaponTimers[weaponId] = new WeaponTimer(weaponModel.AttackInterval);
            return _weaponTimers[weaponId];
        }

        private PlayerWeaponModel CreateModel(ItemId weaponId)
        {
            var config = _weaponConfigs.Get(weaponId.FullName);
            return new PlayerWeaponModel(config);
        }
    }
}