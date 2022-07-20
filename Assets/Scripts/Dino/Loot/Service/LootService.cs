using Dino.Inventory.Service;
using Dino.Loot.Config;
using Feofun.Config;
using Zenject;

namespace Dino.Loot.Service
{
    public class LootService
    {
        [Inject] private InventoryService _inventoryService;
        [Inject] private StringKeyedConfigCollection<LootConfig> _lootConfigs;

        public void Collect(Loot loot)
        {
            var lootConfig = _lootConfigs.Get(loot.ObjectId);
            _inventoryService.Add(lootConfig.ReceivedItemId);
        }
    }
}
