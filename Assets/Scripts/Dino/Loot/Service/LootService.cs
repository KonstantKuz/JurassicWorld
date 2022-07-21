using Dino.Inventory.Service;
using Zenject;

namespace Dino.Loot.Service
{
    public class LootService
    {
        [Inject] private InventoryService _inventoryService;
        [Inject] private ActiveItemService _activeItemService;

        public void Collect(Loot loot)
        {
            var itemId = _inventoryService.Add(loot.ReceivedItemId);
            _activeItemService.Replace(itemId);
        }
    }
}
