using Feofun.Repository;

namespace Dino.Inventory.Service
{
    public class InventoryRepository : LocalPrefsSingleRepository<Model.Inventory>
    {
        public InventoryRepository() : base("Inventory_v3")
        {
        }
    }
}