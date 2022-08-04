using Dino.Inventory.Model;
using Dino.Inventory.Service;
using Dino.Location;
using Dino.Location.Service;
using Dino.Units.Player;
using Dino.Weapon.Service;
using Logger.Extension;
using UniRx;
using Zenject;

namespace Dino.Units.Service
{
    public class ActiveItemService : IWorldScope
    {
        private readonly ReactiveProperty<ItemId> _activeItemId = new ReactiveProperty<ItemId>(null);
        
        [Inject]
        private WorldObjectFactory _worldObjectFactory;
        [Inject]
        private World _world;
        [Inject]
        private WeaponService _weaponService;     
        [Inject]
        private InventoryService _inventoryService;
        
        private ActiveItemRepository _repository;        

        public IReadOnlyReactiveProperty<ItemId> ActiveItemId => _activeItemId;
        
        private PlayerUnit Player => _world.GetPlayer();

        public bool HasActiveItem() => _activeItemId.HasValue && _activeItemId.Value != null;

        public void Init()
        {
            _repository = new ActiveItemRepository();

            if (!_repository.Exists()) {
                _repository.Set(null);
            }
            else
            {
                Equip(_repository.Get());
            }
        }
        public void Replace(ItemId itemId)
        {
            UnEquip();
            Equip(itemId);
        }

        public bool IsActiveItem(ItemId itemId)
        {
            if (!HasActiveItem()) {
                return false;
            }
            return itemId.Equals(_activeItemId.Value);
        }

        public void Equip(ItemId itemId)
        {
            if (!_inventoryService.Contains(itemId)) {
                this.Logger().Error($"Equip error, inventory must contain the item:= {itemId}");
                return;
            }
            if (_activeItemId.Value != null) {
                RemoveActiveItemObject();
            }
            var itemOwner = Player.ActiveItemOwner;
            var itemObject = _worldObjectFactory.CreateObject(itemId.Name, itemOwner.Container);
            _activeItemId.SetValueAndForceNotify(itemId);
            itemOwner.Set(itemObject);
            
            _weaponService.TrySetWeapon(itemId.FullName, itemOwner.GetWeapon());
        }

        public void UnEquip()
        {
            _activeItemId.SetValueAndForceNotify(null);
            RemoveActiveItemObject();
        }

        public void OnWorldSetup()
        {
        }

        public void OnWorldCleanUp()
        {
            RemoveActiveItemObject();
        }

        private void RemoveActiveItemObject()
        {
            _weaponService.Remove();
            Player.ActiveItemOwner.Remove();
        }

        public void Save()
        {
            _repository.Set(ActiveItemId.Value);
        }
    }
}