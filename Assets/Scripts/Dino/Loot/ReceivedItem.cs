using System;
using Dino.Inventory.Model;

namespace Dino.Loot
{
    [Serializable]
    public class ReceivedItem
    {
        public string Id;
        public InventoryItemType Type;
        public int Amount;

        public static ReceivedItem CreateFromItem(Item item)
        {
            return new ReceivedItem
            {
                Id = item.Id.FullName,
                Type = item.Type,
                Amount = item.Amount
            };
        }
    }
}