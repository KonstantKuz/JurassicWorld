using System.Collections.Generic;
using System.Linq;
using Dino.ABTest;
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
        
        private readonly Dictionary<ItemId, WeaponWrapper> _weapons = new Dictionary<ItemId, WeaponWrapper>();
        private readonly InventoryService _inventoryService;
        private readonly StringKeyedConfigCollection<WeaponConfig> _weaponConfigs;
        private readonly World _world;
        private readonly Feofun.ABTest.ABTest _abTest;
        
        private PlayerUnit Player => _world.RequirePlayer();

        public WeaponService(InventoryService inventoryService, StringKeyedConfigCollection<WeaponConfig> weaponConfigs, World world, Feofun.ABTest.ABTest abTest)
        {
            _inventoryService = inventoryService;
            _weaponConfigs = weaponConfigs;
            _abTest = abTest;
            _world = world;
            inventoryService.InventoryProperty.Subscribe(OnInventoryUpdate);
        }

        private void OnInventoryUpdate([CanBeNull] Inventory.Model.Inventory inventory)
        {
            inventory?.GetItems(InventoryItemType.Weapon)
                     .Select(item => item.Id)
                     .ForEach(weaponId => {
                         if (!_weapons.ContainsKey(weaponId)) {
                             _weapons[weaponId] = CreateWeaponWrapper(weaponId);
                         }
                     });
        }

        public void SetWeapon(ItemId itemId, BaseWeapon weaponObject)
        {
            if (IsWeapon(itemId)) {
                SetActiveWeapon(itemId, weaponObject);
            } else {
                this.Logger().Warn($"Inventory item id:= {itemId} is not Weapon");
            }
        }

        public void RemoveActiveWeapon()
        {
            var activeWeapon = Player.PlayerAttack.WeaponWrapper;
            if (activeWeapon == null) {
                return;
            }
            Player.PlayerAttack.DeleteWeapon();
            activeWeapon.WeaponObject = null;
        }
        public bool IsWeapon(ItemId itemId)
        {
            return _weaponConfigs.Contains(itemId.FullName);
        }
        [CanBeNull]
        public WeaponWrapper FindWeaponWrapper(ItemId weaponId) => !IsWeapon(weaponId) ? null : GetWeaponWrapper(weaponId);

        public WeaponWrapper GetWeaponWrapper(ItemId weaponId) =>
                _weapons.ContainsKey(weaponId) ? _weapons[weaponId] : _weapons[weaponId] = CreateWeaponWrapper(weaponId);
        


        private void SetActiveWeapon(ItemId weaponId, BaseWeapon weaponObject)
        {
            var weaponWrapper = GetWeaponWrapper(weaponId);
            weaponWrapper.WeaponObject = weaponObject;
            Player.PlayerAttack.SetWeapon(weaponWrapper);
        }

        private WeaponWrapper CreateWeaponWrapper(ItemId weaponId)
        {
            var model = CreateModel(weaponId);
            var timer = new WeaponTimer(model.AttackInterval);
            var clip = CreateClip(model);
            return WeaponWrapper.Create(weaponId, model, timer, clip);
        }

        private IClip CreateClip(PlayerWeaponModel model)
        {
            if (_abTest.WithoutAmmo()) {
                return new ConstantClip();
            } 
            return new Clip(_inventoryService, ItemId.Create(model.AmmoId));
        }

        private PlayerWeaponModel CreateModel(ItemId weaponId)
        {
            var config = _weaponConfigs.Get(weaponId.FullName);
            return new PlayerWeaponModel(config);
        }
    }
}