using System;
using Dino.Inventory.Components;
using Dino.Location;
using Dino.Location.Service;
using Dino.Units.Player;
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

        private PlayerUnit Player => _world.GetPlayer();

        public void Set(string itemId)
        {
            Remove();
            var itemOwner = Player.ActiveItemOwner;
            var item = _worldObjectFactory.CreateObject(itemId, itemOwner.Container);
            itemOwner.Set(item);
            _weaponService.TrySetWeapon(itemId, itemOwner.GetWeapon());
        }

        public void Remove()
        {
            _weaponService.Remove();
            Player.ActiveItemOwner.Remove();
        }
    }
}