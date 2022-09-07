using System.Collections.Generic;
using System.Linq;
using Dino.Inventory.Model;
using Dino.Inventory.Service;
using Dino.Location;
using Dino.Units.Player;
using Dino.Units.Player.Model;
using Dino.Weapon.Components;
using Dino.Weapon.Config;
using Feofun.Config;
using JetBrains.Annotations;
using Logger.Extension;
using SuperMaxim.Core.Extensions;
using UniRx;

namespace Dino.Weapon.Service
{
    public class WeaponService
    {
        private readonly Dictionary<string, WeaponWrapper> _weapons = new Dictionary<string, WeaponWrapper>();
        private readonly InventoryService _inventoryService;
        private readonly StringKeyedConfigCollection<WeaponConfig> _weaponConfigs;
        private readonly World _world;

        private PlayerUnit Player => _world.RequirePlayer();

        public WeaponService(InventoryService inventoryService, StringKeyedConfigCollection<WeaponConfig> weaponConfigs, World world)
        {
            _inventoryService = inventoryService;
            _weaponConfigs = weaponConfigs;
            _world = world;
            inventoryService.InventoryProperty.Subscribe(OnInventoryUpdate);
        }

        private void OnInventoryUpdate([CanBeNull] Inventory.Model.Inventory inventory)
        {
            inventory?.GetItems(InventoryItemType.Weapon)
                     .Select(item => item.FullName)
                     .ForEach(weaponId => {
                         if (!_weapons.ContainsKey(weaponId)) {
                             _weapons[weaponId] = CreateWeaponWrapper(weaponId);
                         }
                     });
        }

        public void SetActiveWeapon(ItemId itemId, BaseWeapon weapon)
        {
            if (IsWeapon(itemId.FullName)) {
                SetWeapon(itemId.FullName, weapon);
            } else {
                this.Logger().Warn($"Inventory item:= {itemId} is not Weapon");
            }
        }

        public void RemoveActiveWeapon()
        {
            Player.PlayerAttack.DeleteWeapon();
        }

        private bool IsWeapon(string itemId)
        {
            return _weaponConfigs.Contains(itemId);
        }

        public WeaponWrapper GetWeaponWrapper(string weaponId) =>
                _weapons.ContainsKey(weaponId) ? _weapons[weaponId] : _weapons[weaponId] = CreateWeaponWrapper(weaponId);

        [CanBeNull]
        public WeaponWrapper FindWeaponWrapper(string weaponId) => !IsWeapon(weaponId) ? null : GetWeaponWrapper(weaponId);

        private void SetWeapon(string weaponId, BaseWeapon weapon)
        {
            var weaponWrapper = GetWeaponWrapper(weaponId);
            weaponWrapper.Weapon = weapon;
            Player.PlayerAttack.SetWeapon(weaponWrapper);
        }

        private WeaponWrapper CreateWeaponWrapper(string weaponId)
        {
            var model = CreateModel(weaponId);
            var timer = new WeaponTimer(model.AttackInterval);
            var clip = new Clip(_inventoryService, model.AmmoId);
            return WeaponWrapper.Create(weaponId, model, timer, clip);
        }

        private PlayerWeaponModel CreateModel(string weaponId)
        {
            var config = _weaponConfigs.Get(weaponId);
            return new PlayerWeaponModel(config);
        }
    }
}