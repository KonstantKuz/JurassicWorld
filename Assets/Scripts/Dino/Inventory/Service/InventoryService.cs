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

        public event Action<Item> OnItemAdded;
        public event Action<Item> OnItemRemoved;

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
                return CreateNewItem(id, type, amount);
            }
            Assert.IsTrue(item.Type == type, $"Error adding item, type:= {type} must equal type of inventory itemId:= {id}");
            item.IncreaseAmount(amount);
            Set(Inventory);
            return item;
        }

        public void DecreaseItemAmount(ItemId id, int amount)
        {
            var item = GetItem(id);
            var inventory = Inventory;
            item.DecreaseAmount(amount);
            if (item.Amount <= 0) {
                inventory.Remove(id);
            }
            Set(inventory);
        }

        public void Remove(ItemId id)
        {
            var inventory = Inventory;
            inventory.Remove(id);
            Set(inventory);
            OnItemRemoved?.Invoke(id);
        }

        private Item CreateNewItem(ItemId id, InventoryItemType type, int amount)
        {
            var inventory = Inventory;
            var item = Item.Create(id, type, amount);
            inventory.Create(id, item);
            Set(inventory);
            OnItemAdded?.Invoke(item);
            return item;
        }

        private void Set(Model.Inventory model)
        {
            _inventory.SetValueAndForceNotify(model);
        }
    }
}