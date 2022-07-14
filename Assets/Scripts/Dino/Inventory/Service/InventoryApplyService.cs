using Dino.Extension;
using Dino.Inventory.Components;
using Dino.Location;
using Dino.Location.Service;
using Dino.Units;
using Dino.Weapon.Service;
using Zenject;

namespace Dino.Inventory.Service
{
    public class InventoryApplyService
    {
        [Inject]
        private WorldObjectFactory _worldObjectFactory;

        [Inject]
        private World _world;    
        [Inject]
        private WeaponService _weaponService;

        private Unit Player => _world.GetPlayer();

        public void Set(string inventoryId)
        {
            Remove();
            var inventoryOwner = Player.GameObject.RequireComponent<InventoryOwner>();
            var item = _worldObjectFactory.CreateObject(inventoryId, inventoryOwner.Container);
            inventoryOwner.Set(item);
            _weaponService.Set(inventoryId);
        }

        private void Remove()
        {
            _weaponService.Remove();
            var inventoryOwner = Player.GameObject.RequireComponent<InventoryOwner>();
            inventoryOwner.Remove();
        }

    }
}