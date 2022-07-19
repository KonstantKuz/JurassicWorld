using System.Collections.Generic;
using Logger.Extension;

namespace Dino.Inventory.Model
{
    public class Inventory
    {
        public List<string> Items { get; } = new List<string>();

        public void Add(string itemId)
        {
            Items.Add(itemId);
        }
        public bool Contains(string itemId) => Items.Contains(itemId);

        public void Remove(string itemId)
        {
            if (!Items.Contains(itemId)) {
                this.Logger().Error($"Inventory remove error, inventory doesn't contain item:= {itemId},");
                return;
            }
            Items.Add(itemId);
        }
    }
}