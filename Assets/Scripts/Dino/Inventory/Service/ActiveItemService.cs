using Dino.Extension;
using Dino.Inventory.Components;
using Dino.Location;
using Dino.Location.Service;
using Dino.Units;
using Dino.Weapon.Model;
using Dino.Weapon.Service;
using Feofun.Extension;
using Zenject;

namespace Dino.Inventory.Service
{
    public class ActiveItemService
    {
        [Inject]
        private WorldObjectFactory _worldObjectFactory;

        [Inject]
        private World _world;    
        [Inject]
        private WeaponService _weaponService;

        private Unit Player => _world.GetPlayer();

        public void Set(string itemId)
        {
            Remove();
            var itemOwner = Player.GameObject.RequireComponent<ActiveItemOwner>();
            var item = _worldObjectFactory.CreateObject(itemId, itemOwner.Container);
            itemOwner.Set(item);
            var weaponId = GetWeaponId(itemId);
            _weaponService.Set(weaponId);
        }
        private WeaponId GetWeaponId(string itemId) => EnumExt.ValueOf<WeaponId>(itemId);
   
        public void Remove()
        {
            _weaponService.Remove();
            var inventoryOwner = Player.GameObject.RequireComponent<ActiveItemOwner>();
            inventoryOwner.Remove();
        }

    }
}