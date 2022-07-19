using Dino.Inventory.Model;
using Dino.Location;
using Dino.Location.Service;
using Dino.Units.Player;
using Dino.Weapon.Service;
using Logger.Extension;
using UniRx;
using Zenject;

namespace Dino.Inventory.Service
{
    public class ActiveItemService
    {
        private readonly ReactiveProperty<InventoryItem> _activeItemId = new ReactiveProperty<InventoryItem>(null);
        
        [Inject]
        private WorldObjectFactory _worldObjectFactory;
        [Inject]
        private World _world;
        [Inject]
        private WeaponService _weaponService;     
        [Inject]
        private InventoryService _inventoryService;

        public IReadOnlyReactiveProperty<InventoryItem> ActiveItemId => _activeItemId;
        
        private PlayerUnit Player => _world.GetPlayer();

        public bool HasActiveItem() => _activeItemId.HasValue && _activeItemId.Value != null;
        public void Replace(InventoryItem item)
        {
            UnEquip();
            Equip(item);
        }

        public void Equip(InventoryItem item)
        {
            if (_activeItemId.Value != null) {
                this.Logger().Error($"Equip error, active item:= {item} is not null, should unEquip the previous active item of unit");
                return;
            }
            if (!_inventoryService.Contains(item)) {
                this.Logger().Error($"Equip error, inventory must contain the item:= {item}");
                return;
            }
            var itemOwner = Player.ActiveItemOwner;
            var itemObject = _worldObjectFactory.CreateObject(item.Id, itemOwner.Container);
            _activeItemId.SetValueAndForceNotify(item);
            itemOwner.Set(itemObject);
            
            _weaponService.TrySetWeapon(item.Id, itemOwner.GetWeapon());
        }

        public void UnEquip()
        {
            _activeItemId.SetValueAndForceNotify(null);
            _weaponService.Remove();
            Player.ActiveItemOwner.Remove();
        }
    }
}