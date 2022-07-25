﻿using System;
using System.Collections.Generic;
using System.Linq;
using Dino.Inventory.Model;
using Dino.Location;
using ModestTree;
using UniRx;

namespace Dino.Inventory.Service
{
    public class InventoryService : IWorldScope
    {
        private readonly ReactiveProperty<Model.Inventory> _inventory = new ReactiveProperty<Model.Inventory>(null);
        
        private readonly InventoryRepository _repository;
        
        public IReadOnlyReactiveProperty<Model.Inventory> InventoryProperty => _inventory;
        
        private Model.Inventory Inventory => _repository.Get();

        public InventoryService(InventoryRepository repository)
        {
            _repository = repository;
            if (!_repository.Exists()) {
                _repository.Set(new Model.Inventory());
            }
            _inventory.SetValueAndForceNotify(Inventory);
            
        }
        public bool HasInventory() => _repository.Exists() && _inventory.HasValue && _inventory.Value != null;
        public bool Contains(ItemId id) => Inventory.Contains(id);
        public int Count(string itemName) => Inventory.Items.Count(it => it.Name == itemName);

        public ItemId Add(string itemName)
        {
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
            var items = _inventory.Value.Items.Where(it => it.Name == itemName);
            if (items.IsEmpty()) {
                throw new NullReferenceException($"Error getting items, inventory doesn't contain items:= {itemName}");
            }
            return items;
        }
        public ItemId GetLast(string itemName)
        {
            var itemId = _inventory.Value.Items.Where(it => it.Name == itemName).OrderBy(it => it.Number).LastOrDefault();
            if (itemId == null) {
                throw new NullReferenceException($"Error getting last item, inventory doesn't contain item name:= {itemName}");
            }
            return itemId;
        }

        private ItemId CreateNewId(string itemName)
        {
            var items = Inventory.Items.Where(it => it.Name == itemName);
            if (items.IsEmpty()) {
                return ItemId.Create(itemName, 1);
            }
            var number = items.Max(it => it.Number) + 1;
            return ItemId.Create(itemName, number);
        }

        private void Set(Model.Inventory model)
        {
            _inventory.SetValueAndForceNotify(model);
        }

        public void OnWorldSetup()
        {
        
        }
        public void OnWorldCleanUp()
        {
            _repository.Set(Inventory);
        }
    }
}