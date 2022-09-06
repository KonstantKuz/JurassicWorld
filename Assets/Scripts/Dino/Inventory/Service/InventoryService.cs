using System;
using Dino.Inventory.Model;
using Dino.Location;
using UniRx;
using UnityEngine.Assertions;

namespace Dino.Inventory.Service
{
    public class InventoryService : IWorldScope
    {
        private readonly ReactiveProperty<Model.Inventory> _inventory = new ReactiveProperty<Model.Inventory>(null);

        private InventoryRepository _repository = new InventoryRepository();
        public IReadOnlyReactiveProperty<Model.Inventory> InventoryProperty => _inventory;

        private Model.Inventory Inventory => _repository.Get();
        public int ItemsCount => Inventory.Items.Count;

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

        public ItemId FindItem(string itemName) => Inventory.FindItem(itemName);

        public ItemId GetItem(string itemName)
        {
            var itemId = FindItem(itemName);
            if (itemId == null) { 
                throw new NullReferenceException($"Error getting item, inventory doesn't contain item:= {itemName}");
            }
            return itemId;
        }

        public int Count(string itemName)
        {
            var item = FindItem(itemName);
            return item?.Amount ?? 0;
        }

        public ItemId Add(string itemName, InventoryItemType type, int amount)
        {
            var itemId = FindItem(itemName);
            if (itemId == null) { 
                return CreateNewItem(itemName, type, amount);
            }
            Assert.IsTrue(itemId.Type == type, $"Error adding item, type:= {type} must equal type of inventory item:= {itemId}");
            itemId.IncreaseAmount(amount);
            Set(Inventory);
            return itemId;
        }
        
        public void DecreaseItems(string itemName, int amount)
        {
            var itemId = GetItem(itemName);
            var inventory = Inventory;
            itemId.DecreaseAmount(amount);
            if (itemId.Amount <= 0) {
                inventory.Remove(itemId);
            }
            Set(inventory);
        }

        public void Remove(ItemId id)
        {
            var inventory = Inventory;
            inventory.Remove(id);
            Set(inventory);
        }
        private ItemId CreateNewItem(string itemName, InventoryItemType type, int amount)
        {
            var inventory = Inventory;
            var itemId = ItemId.Create(itemName, type, amount);
            inventory.Add(itemId);
            Set(inventory);
            return itemId;
        }

        private void Set(Model.Inventory model)
        {
            _inventory.SetValueAndForceNotify(model);
        }
    }
}