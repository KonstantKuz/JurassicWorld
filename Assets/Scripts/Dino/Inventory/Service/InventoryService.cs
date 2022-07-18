using Zenject;

namespace Dino.Inventory.Service
{
    public class InventoryService
    {
        [Inject]
        private InventoryRepository _repository;

        public Model.Inventory Inventory
        {
            get
            {
                if (!_repository.Exists()) {
                    _repository.Set(new Model.Inventory());
                }
                return _repository.Get();
            }
        }
        public void Add(string upgradeId)
        {
            var inventory = Inventory;
            inventory.UnitsUpgrades.AddUpgrade(upgradeId);
            Set(inventory);
        }     
        public void Remove(string upgradeId)
        {
            var inventory = Inventory;
            inventory.UnitsUpgrades.AddUpgrade(upgradeId);
            Set(inventory);
        }

        public void Equip(string upgradeId)
        {
            
        } 
        public void Equip(string upgradeId)
        {
            
        }

        private void Set(Model.Inventory model)
        {
            _repository.Set(model);
        }
    }
}