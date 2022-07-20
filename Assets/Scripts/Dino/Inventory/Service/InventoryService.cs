using System;
using System.Linq;
using Dino.Inventory.Model;
using Dino.Location;
using Dino.Weapon.Config;
using Dino.Weapon.Model;
using Feofun.Config;
using ModestTree;
using SuperMaxim.Core.Extensions;
using UniRx;
using Zenject;

namespace Dino.Inventory.Service
{
    public class InventoryService : IWorldScope
    {
        private readonly ReactiveProperty<Model.Inventory> _inventory = new ReactiveProperty<Model.Inventory>(null);

        [Inject]
        private InventoryRepository _repository;

        public IReadOnlyReactiveProperty<Model.Inventory> InventoryProperty => _inventory;
        
        private Model.Inventory Inventory => _repository.Get();

        public void OnWorldSetup()
        {
            _repository.Set(new Model.Inventory());
            _inventory.SetValueAndForceNotify(Inventory);
        }

        public bool HasInventory() => _repository.Exists() && _inventory.HasValue && _inventory.Value != null;
        public bool Contains(ItemId id) => Inventory.Contains(id);

        public void Add(string itemName)
        {
            var inventory = Inventory;
            var itemId = CreateNewId(itemName);
            inventory.Add(itemId);
            Set(inventory);
        }

        public void Remove(ItemId id)
        {
            var inventory = Inventory;
            inventory.Remove(id);
            Set(inventory);
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
            _repository.Set(model);
            _inventory.SetValueAndForceNotify(model);
        }

        public void OnWorldCleanUp()
        {
            _inventory.SetValueAndForceNotify(null);
            _repository.Delete();
        }
    }
}