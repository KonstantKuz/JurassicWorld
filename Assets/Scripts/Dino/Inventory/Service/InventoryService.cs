using System;
using System.Linq;
using Dino.Inventory.Model;
using Dino.Location;
using Dino.Weapon.Config;
using Dino.Weapon.Model;
using Feofun.Config;
using Logger.Extension;
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

        [Inject]
        private ConfigCollection<WeaponId, WeaponConfig> _weaponConfigs;
        private Model.Inventory Inventory => _repository.Get();

        public void OnWorldSetup()
        {
            _repository.Set(new Model.Inventory());
            _inventory.SetValueAndForceNotify(Inventory);
            InitForTest();
        }

        //todo remove after adding inventory ui
        private void InitForTest()
        {
            _weaponConfigs.ForEach(it => Add(it.Id.ToString()));
        }
        public bool HasInventory() => _repository.Exists() && _inventory.HasValue && _inventory.Value != null;
        public InventoryItem Get(string itemId, int number)
        {
            var item = new InventoryItem(itemId, number);
            if (!Contains(item)) {
                throw new NullReferenceException($"Inventory Get error, inventory doesn't contain item:= {item}");
            }
            return Find(itemId, number);
        }

        public InventoryItem Find(string itemId, int number)
        {
            return Inventory.Items.FirstOrDefault(it => it.Equals(new InventoryItem(itemId, number)));
        }
        public bool Contains(InventoryItem item) => Inventory.Contains(item);
        
        public void Add(string itemId)
        {
            var inventory = Inventory;
            var item = CreateNewItem(itemId);
            inventory.Add(item);
            Set(inventory);
        }
        public void Remove(string itemId, int number)
        {
            Remove(new InventoryItem(itemId, number));
        }       
        public void RemoveLast(string itemId)
        {
            var item = _inventory.Value.Items.LastOrDefault(it => it.Id == itemId);
            if (item == null) {
                this.Logger().Error($"Inventory remove error, inventory doesn't contain itemId:= {itemId}");
                return;
            }
            Remove(item);
        }  
        public void Remove(InventoryItem item)
        {
            var inventory = Inventory;
            inventory.Remove(item);
            Set(inventory);
        }
        private InventoryItem CreateNewItem(string id)
        {
            var number = Inventory.Items.Count(it => it.Id == id) + 1;
            return new InventoryItem(id, number);
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