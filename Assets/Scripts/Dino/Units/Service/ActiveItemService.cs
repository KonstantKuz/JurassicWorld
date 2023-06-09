﻿using Dino.Inventory.Model;
using Dino.Inventory.Service;
using Dino.Location;
using Dino.Location.Service;
using Dino.Units.Player;
using Dino.Weapon.Service;
using Logger.Extension;
using UniRx;
using Zenject;

namespace Dino.Units.Service
{
    public class ActiveItemService
    {
        private readonly ReactiveProperty<Item> _activeItemId = new ReactiveProperty<Item>(null);
        private readonly InventoryService _inventoryService;

        [Inject]
        private WorldObjectFactory _worldObjectFactory;
        [Inject]
        private World _world;
        [Inject]
        private WeaponService _weaponService;

        private ActiveItemRepository _repository;

        public IReadOnlyReactiveProperty<Item> ActiveItemId => _activeItemId;
        private PlayerUnit Player => _world.RequirePlayer();

        public ActiveItemService(InventoryService inventoryService)
        {
            _inventoryService = inventoryService;
            inventoryService.OnItemChanged += OnItemChanged;
        }

        public void Init()
        {
            _repository = new ActiveItemRepository();

            if (!_repository.Exists()) {
                _repository.Set(null);
            } else {
                Equip(_repository.Get());
            }
        }

        public void Save()
        {
            _repository.Set(ActiveItemId.Value);
        }

        public bool HasActiveItem() => _activeItemId.HasValue && _activeItemId.Value != null;
        public bool IsActiveItem(ItemId itemId) => HasActiveItem() && itemId.Equals(_activeItemId.Value.Id);

        public void Replace(Item item)
        {
            UnEquip();
            Equip(item);
        }

        public void Equip(Item item)
        {
            if (!_inventoryService.Contains(item.Id)) {
                this.Logger().Error($"Equip error, inventory must contain the item:= {item}");
                return;
            }
            if (!IsItemTypeEquipable(item)) {
                this.Logger().Error($"Equip error, type of inventory item must be equipable, item:= {item}");
                return;
            }
            if (_activeItemId.Value != null) {
                RemoveActiveItemObject();
            }
            var itemOwner = Player.ActiveItemOwner;
            var itemObject = _worldObjectFactory.CreateObject(item.Name, itemOwner.Container);
            itemOwner.Set(itemObject);

            _weaponService.SetWeapon(item.Id, itemOwner.GetWeapon());
            _activeItemId.SetValueAndForceNotify(item);
        }

        public void UnEquip()
        {
            RemoveActiveItemObject();
            _activeItemId.SetValueAndForceNotify(null);
        }

        public void RemoveActiveItemObject()
        {
            _weaponService.RemoveActiveWeapon();
            Player.ActiveItemOwner.Remove();
        }

        private void OnItemChanged(ItemChangedEvent itemChangedEvent)
        {
            if (itemChangedEvent.IsLastItemRemoved) {
                OnItemRemoved(itemChangedEvent.ItemId);
            }
            if (itemChangedEvent.IsItemAddedAsNew) {
                TryEquipAddedItem(itemChangedEvent.ItemId);
            }
        }

        private void OnItemRemoved(ItemId id)
        {
            if (IsActiveItem(id)) {
                UnEquip();
            }
        }
        private void TryEquipAddedItem(ItemId itemId)
        {
            var newItem = _inventoryService.GetItem(itemId);
            if (!IsItemTypeEquipable(newItem)) {
                return;
            }
            if (!HasActiveItem() || ShouldEquipAddedItem(ActiveItemId.Value, newItem)) {
                Replace(newItem);
            }
        }

        private bool ShouldEquipAddedItem(Item currentItem, Item nextItem)
        {
            if (!_weaponService.IsWeapon(currentItem.Id) || !_weaponService.IsWeapon(nextItem.Id)) {
                return nextItem.Rank >= currentItem.Rank;
            }
            var currentItemAmmoCount = _weaponService.GetWeaponWrapper(currentItem.Id).Clip.AmmoCount.Value;
            var nextItemAmmoCount = _weaponService.GetWeaponWrapper(nextItem.Id).Clip.AmmoCount.Value;
            if (currentItemAmmoCount <= 0 && nextItemAmmoCount > 0) {
                return true;
            }
            if (currentItemAmmoCount <= 0) {
                return true;
            }   
            if (nextItemAmmoCount <= 0) {
                return false;
            }
            return nextItem.Rank >= currentItem.Rank;
        }

        private bool IsItemTypeEquipable(Item item) => item.Type.IsEquipable();

     
    }
}