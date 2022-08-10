using System.Collections.Generic;
using Logger.Extension;

namespace Dino.Inventory.Model
{
    public class Inventory
    {
        public List<ItemId> Items { get; } = new List<ItemId>();

        public void Add(ItemId id)
        {
            if (Contains(id)) {
                this.Logger().Error($"Inventory adding error, inventory already contains item id:= {id}");
                return;
            }
            Items.Add(id);
        }
        
        public bool Contains(ItemId id) => Items.Contains(id);

        public void Remove(ItemId id)
        {
            if (!Contains(id)) {
                this.Logger().Error($"Inventory remove error, inventory doesn't contain item id:= {id}");
                return;
            }
            Items.Remove(id);
        }
    }
}