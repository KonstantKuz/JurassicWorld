using Dino.Location;
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

        private Model.Inventory Inventory => _repository.Get();

        public void OnWorldSetup()
        {
            _repository.Set(new Model.Inventory());
            _inventory.SetValueAndForceNotify(Inventory);
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