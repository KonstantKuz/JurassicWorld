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
        private readonly StringReactiveProperty _activeItemId = new StringReactiveProperty();
        
        [Inject]
        private WorldObjectFactory _worldObjectFactory;
        [Inject]
        private World _world;
        [Inject]
        private WeaponService _weaponService;     
        [Inject]
        private InventoryService _inventoryService;

        public IReadOnlyReactiveProperty<string> ActiveItemId => _activeItemId;
        
        private PlayerUnit Player => _world.GetPlayer();

        public void Change(string itemId)
        {
            UnEquip();
            Equip(itemId);
        }

        public void Equip(string itemId)
        {
            if (!_inventoryService.Contains(itemId)) {
                this.Logger().Error($"Equip error, inventory must contain the item:= {itemId}");
                return;
            }
            if (_activeItemId.Value != null) {
                this.Logger().Error($"Equip error, active item:= {itemId} is not null, should unEquip the previous active item of unit");
                return;
            }
            _activeItemId.SetValueAndForceNotify(itemId);
            var itemOwner = Player.ActiveItemOwner;
            var item = _worldObjectFactory.CreateObject(itemId, itemOwner.Container);
            itemOwner.Set(item);
            
            _weaponService.TrySetWeapon(itemId, itemOwner.GetWeapon());
        }

        public void UnEquip()
        {
            _activeItemId.SetValueAndForceNotify(null);
            _weaponService.Remove();
            Player.ActiveItemOwner.Remove();
        }
    }
}