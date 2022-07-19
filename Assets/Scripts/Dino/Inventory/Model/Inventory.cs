using System.Collections.Generic;
using Logger.Extension;

namespace Dino.Inventory.Model
{
    public class Inventory
    {
        public List<InventoryItem> Items { get; } = new List<InventoryItem>();

        public void Add(InventoryItem item)
        {
            Items.Add(item);
        }
        
        public bool Contains(InventoryItem item) => Items.Contains(item);

        public void Remove(InventoryItem item)
        {
            if (!Items.Contains(item)) {
                this.Logger().Error($"Inventory remove error, inventory doesn't contain item:= {item}");
                return;
            }
            Items.Remove(item);
        }
    }
}