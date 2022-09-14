using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Logger.Extension;
using Newtonsoft.Json;

namespace Dino.Inventory.Model
{
    public class Inventory
    {
        [JsonProperty]
        private readonly List<Item> _items = new List<Item>();
        public int GetUniqueItemsCount(InventoryItemType type) => _items.Count(it => it.Type == type);
        public IEnumerable<Item> GetItems(InventoryItemType type) => _items.Where(it => it.Type == type);

        public void AddNewItem(Item item)
        {
            if (Contains(item.Id)) {
                this.Logger().Error($"Inventory adding error, inventory already contains item id:= {item.Id}");
                return;
            }
            _items.Add(item);
        }

        public void RemoveItem(ItemId id)
        {
            if (!Contains(id)) {
                this.Logger().Error($"Inventory remove error, inventory doesn't contain item id:= {id}");
                return;
            }
            _items.RemoveAll(it => it.Id.Equals(id));
        }

        [CanBeNull]
        public Item FindItem(ItemId id) => _items.FirstOrDefault(it => it.Id.Equals(id));

        public bool Contains(ItemId id) => _items.Exists(it => it.Id.Equals(id));
    }
}