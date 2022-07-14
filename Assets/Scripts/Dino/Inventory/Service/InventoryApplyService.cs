using Dino.Extension;
using Dino.Inventory.Components;
using Dino.Location;
using Dino.Location.Service;
using ModestTree;
using Zenject;

namespace Dino.Inventory.Service
{
    public class InventoryApplyService
    {
        [Inject]
        private WorldObjectFactory _worldObjectFactory;
        [Inject] private World _world;
        
        public void Apply(string inventoryId)
        {
            CheckPlayer();
            var player = _world.Player;
            var inventoryOwner = player.GameObject.RequireComponent<InventoryOwner>();
            var item = _worldObjectFactory.CreateObject(inventoryId, inventoryOwner.transform);
            inventoryOwner.SetInventory(item);

        }
        private void CheckPlayer() => Assert.IsNotNull(_world.Player, "Squad is null, should call this method only inside game session");
    }
}