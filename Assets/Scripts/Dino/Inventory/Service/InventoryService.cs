using System;
using System.Collections.Generic;
using Dino.Inventory.Model;
using Dino.Location;
using JetBrains.Annotations;
using UniRx;
using UnityEngine.Assertions;

namespace Dino.Inventory.Service
{
    public class InventoryService : IWorldScope
    {
        public const int MAX_UNIQUE_WEAPONS_COUNT = 4;

        private readonly ReactiveProperty<Model.Inventory> _inventory = new ReactiveProperty<Model.Inventory>(null);
        private InventoryRepository _repository = new InventoryRepository();

        public IReadOnlyReactiveProperty<Model.Inventory> InventoryProperty => _inventory;
        private Model.Inventory Inventory => _repository.Get();

        public event Action<ItemChangedEvent> OnItemChanged;

        public int GetUniqueItemsCount(InventoryItemType type) => Inventory.GetUniqueItemsCount(type);
        public IEnumerable<Item> GetItems(InventoryItemType type) => Inventory.GetItems(type);

        public void OnWorldSetup()
        {
            _repository = new InventoryRepository();

            if (!_repository.Exists()) {
                _repository.Set(new Model.Inventory());
            }
            _inventory.SetValueAndForceNotify(Inventory);
        }

        public void OnWorldCleanUp()
        {
        }

        public void Save()
        {
            _repository.Set(Inventory);
        }

        public bool HasInventory() => _repository.Exists() && _inventory.HasValue && _inventory.Value != null;
        public bool Contains(ItemId id) => Inventory.Contains(id);

        [CanBeNull]
        public Item FindItem(ItemId id) => Inventory.FindItem(id);

        public Item GetItem(ItemId id)
        {
            var itemId = FindItem(id);
            if (itemId == null) {
                throw new NullReferenceException($"Error getting item, inventory doesn't contain itemId:= {id}");
            }
            return itemId;
        }

        public int GetAmount(ItemId id)
        {
            var item = FindItem(id);
            return item?.Amount ?? 0;
        }

        public Item Add(ItemId id, InventoryItemType type, int amount)
        {
            var item = FindItem(id);
            if (item == null) {
                return AddNewItemInInventory(id, type, amount);
            }
            Assert.IsTrue(item.Type == type, $"Error adding item, type:= {type} must equal type of inventory itemId:= {id}");
            var previousAmount = item.Amount;
            item.IncreaseAmount(amount);
            Set(Inventory);
            OnItemChanged?.Invoke(new ItemChangedEvent(id, previousAmount, item.Amount));
            return item;
        }
        public void Remove(ItemId id, int amount)
        {
            var item = GetItem(id);
            var previousAmount = item.Amount;
            var inventory = Inventory;
            item.DecreaseAmount(amount);
            if (item.Amount <= 0) {
                inventory.RemoveItem(id);
            }
            OnItemChanged?.Invoke(new ItemChangedEvent(id, previousAmount, item.Amount));
            Set(inventory);
        }
        public void Remove(ItemId id)
        {
            var item = GetItem(id);
            Remove(id, item.Amount);
        }

        private Item AddNewItemInInventory(ItemId id, InventoryItemType type, int amount)
        {
            var inventory = Inventory;
            var item = Item.Create(id, type, amount);
            inventory.AddNewItem(item);
            Set(inventory);
            OnItemChanged?.Invoke(new ItemChangedEvent(id, 0, item.Amount));
            return item;
        }

        private void Set(Model.Inventory model)
        {
            _inventory.SetValueAndForceNotify(model);
        }
    }
}