using System.Collections.Generic;
using Dino.Craft.Config;
using Dino.Inventory.Model;
using Dino.Inventory.Service;
using Zenject;

namespace Dino.Craft.Service
{
    public class CraftService
    {
        [Inject]
        public CraftConfig _craftConfig;      
        
        [Inject]
        public InventoryService _inventoryService;
        public List<CraftRecipeConfig> GetPossibleCrafts(List<ItemId> items)
        {
            
        }
        
        
    }
}