using Dino.Extension;
using Dino.Inventory.Components;
using Dino.Location;
using Dino.Location.Service;
using Dino.Units;
using Dino.Units.Player.Attack;
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

        public void Apply(string inventoryId)
        {
            Delete();
            var inventoryOwner = Player.GameObject.RequireComponent<InventoryOwner>();
            var item = _worldObjectFactory.CreateObject(inventoryId, inventoryOwner.Container);
            inventoryOwner.Set(item);
        }

        public void Delete()
        {
            var attack = Player.GameObject.RequireComponent<PlayerAttack>();
            attack.DeleteWeapon();
            var inventoryOwner = Player.GameObject.RequireComponent<InventoryOwner>();
            inventoryOwner.Delete();
        }

        private void AddWeapon(string inventoryId)
        {
            
        }
        
    }
}