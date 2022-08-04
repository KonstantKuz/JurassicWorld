using Dino.Inventory.Model;
using Feofun.Repository;

namespace Dino.Units.Service
{
    public class ActiveItemRepository: LocalPrefsSingleRepository<ItemId>
    {
        public ActiveItemRepository(): base("ActiveItem_v1")
        {
        }
    }
}