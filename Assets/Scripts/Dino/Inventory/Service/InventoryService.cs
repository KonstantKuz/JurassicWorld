using Dino.Location;
using Dino.Weapon.Config;
using Dino.Weapon.Model;
using Feofun.Config;
using SuperMaxim.Core.Extensions;
using UniRx;
using Zenject;

namespace Dino.Inventory.Service
{
    public class InventoryService : IWorldScope
    {
        private readonly ReactiveProperty<Model.Inventory> _inventory = new ReactiveProperty<Model.Inventory>();

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

        public bool Contains(string itemId) => Inventory.Contains(itemId);

        public void Add(string itemId)
        {
            var inventory = Inventory;
            inventory.Add(itemId);
            Set(inventory);
        }

        public void Remove(string itemId)
        {
            var inventory = Inventory;
            inventory.Remove(itemId);
            Set(inventory);
        }

        private void Set(Model.Inventory model)
        {
            _repository.Set(model);
        }

        public void OnWorldCleanUp()
        {
            _inventory.Value = null;
            _repository.Delete();
        }
    }
}