using System;
using System.Collections.Generic;
using System.Linq;
using Dino.Inventory.Model;
using Dino.Location;
using Logger.Extension;
using ModestTree;
using UniRx;

namespace Dino.Inventory.Service
{
    public class InventoryService : IWorldScope
    {
        public const int MAX_ITEMS_COUNT = 4;
        
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
        public int Count(string itemName) => Inventory.Items.Count(it => it.FullName == itemName);

        public ItemId Add(string itemName)
        {
            if (ItemsCount >= MAX_ITEMS_COUNT) {
                throw new Exception($"Can't add item {itemName} because inventory is full. ");
            }
            
            var inventory = Inventory;
            var itemId = CreateNewId(itemName);
            inventory.Add(itemId);
            Set(inventory);
            return itemId;
        }

        public void Remove(ItemId id)
        {
            var inventory = Inventory;
            inventory.Remove(id);
            Set(inventory);
        }
        public IEnumerable<ItemId> GetAll(string itemName)
        {
            var items = _inventory.Value.Items.Where(it => it.FullName == itemName).ToList();
            if (items.IsEmpty()) {
                throw new NullReferenceException($"Error getting items, inventory doesn't contain items:= {itemName}");
            }
            return items;
        }
        public ItemId GetLast(string itemName)
        {
            var itemId = _inventory.Value.Items.Where(it => it.FullName == itemName).OrderBy(it => it.Amount).LastOrDefault();
            if (itemId == null) {
                throw new NullReferenceException($"Error getting last item, inventory doesn't contain item name:= {itemName}");
            }
            return itemId;
        }

        private ItemId CreateNewId(string itemName)
        {
            var items = Inventory.Items.Where(it => it.FullName == itemName).ToList();
            if (items.IsEmpty()) {
                return ItemId.Create(itemName, 1);
            }
            var count = items.Max(it => it.Amount) + 1;
            return ItemId.Create(itemName, count);
        }

        private void Set(Model.Inventory model)
        {
            _inventory.SetValueAndForceNotify(model);
        }
    }
}