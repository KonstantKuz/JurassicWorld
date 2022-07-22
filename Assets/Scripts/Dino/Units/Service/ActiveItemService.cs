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

        public IReadOnlyReactiveProperty<ItemId> ActiveItemId => _activeItemId;
        
        private PlayerUnit Player => _world.GetPlayer();

        public bool HasActiveItem() => _activeItemId.HasValue && _activeItemId.Value != null;
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
            if (_activeItemId.Value != null) {
                this.Logger().Error($"Equip error, active item:= {itemId} is not null, should unEquip the previous active item of unit");
                return;
            }
            if (!_inventoryService.Contains(itemId)) {
                this.Logger().Error($"Equip error, inventory must contain the item:= {itemId}");
                return;
            }
            var itemOwner = Player.ActiveItemOwner;
            var itemObject = _worldObjectFactory.CreateObject(itemId.Name, itemOwner.Container);
            _activeItemId.SetValueAndForceNotify(itemId);
            itemOwner.Set(itemObject);
            
            _weaponService.TrySetWeapon(itemId.Name, itemOwner.GetWeapon());
        }

        public void UnEquip()
        {
            _activeItemId.SetValueAndForceNotify(null);
            _weaponService.Remove();
            Player.ActiveItemOwner.Remove();
        }

        public void OnWorldSetup()
        {
        }

        public void OnWorldCleanUp()
        {
            UnEquip();
        }
    }
}