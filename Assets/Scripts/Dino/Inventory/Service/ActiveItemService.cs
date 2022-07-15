using System;
using Dino.Extension;
using Dino.Inventory.Components;
using Dino.Location;
using Dino.Location.Service;
using Dino.Units;
using Dino.Weapon.Model;
using Dino.Weapon.Service;
using Logger.Extension;
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
            
            if (IsWeapon(itemId, out var weaponId)) {
                _weaponService.Set(weaponId);
            } else {
                this.Logger().Debug($"Active Item:= {itemId} is not Weapon");
            }
    
        }
        private bool IsWeapon(string itemId, out WeaponId weaponId)
        {
            return Enum.TryParse(itemId, out weaponId);
        }
        public void Remove()
        {
            _weaponService.Remove();
            var inventoryOwner = Player.GameObject.RequireComponent<ActiveItemOwner>();
            inventoryOwner.Remove();
        }

    }
}