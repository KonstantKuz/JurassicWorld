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
        public void Replace(ItemId id)
        {
            UnEquip();
            Equip(id);
        }

        public void Equip(ItemId id)
        {
            if (_activeItemId.Value != null) {
                this.Logger().Error($"Equip error, active item:= {id} is not null, should unEquip the previous active item of unit");
                return;
            }
            if (!_inventoryService.Contains(id)) {
                this.Logger().Error($"Equip error, inventory must contain the item:= {id}");
                return;
            }
            var itemOwner = Player.ActiveItemOwner;
            var itemObject = _worldObjectFactory.CreateObject(id.Name, itemOwner.Container);
            _activeItemId.SetValueAndForceNotify(id);
            itemOwner.Set(itemObject);
            
            _weaponService.TrySetWeapon(id.Name, itemOwner.GetWeapon());
        }

        public void UnEquip()
        {
            _activeItemId.SetValueAndForceNotify(null);
            _weaponService.Remove();
            Player.ActiveItemOwner.Remove();
        }
    }
}