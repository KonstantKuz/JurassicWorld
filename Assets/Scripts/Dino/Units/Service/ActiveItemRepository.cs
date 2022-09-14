using Dino.Inventory.Model;
using Feofun.Repository;

namespace Dino.Units.Service
{
    public class ActiveItemRepository: LocalPrefsSingleRepository<Item>
    {
        public ActiveItemRepository(): base("ActiveItem_v1")
        {
        }
    }
}