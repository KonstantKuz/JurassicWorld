﻿using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Logger.Extension;

namespace Dino.Inventory.Model
{
    public class Inventory
    {
        private List<KeyValuePair<ItemId, Item>> Items { get; } = new List<KeyValuePair<ItemId, Item>>();
        public int GetUniqueItemsCount(InventoryItemType type) => Items.Count(it => it.Value.Type == type);
        public IEnumerable<Item> GetItems(InventoryItemType type) => Items.Where(it => it.Value.Type == type)
                                                                          .Select(it => it.Value);

        public void Create(ItemId id, Item item)
        {
            if (Contains(id)) {
                this.Logger().Error($"Inventory adding error, inventory already contains item id:= {id}");
                return;
            }
            Items.Add(new KeyValuePair<ItemId, Item>(id, item));
        }

        public void Remove(ItemId id)
        {
            if (!Contains(id)) {
                this.Logger().Error($"Inventory remove error, inventory doesn't contain item id:= {id}");
                return;
            }
            Items.RemoveAll(it => it.Key.Equals(id));
        }

        [CanBeNull]
        public Item FindItem(ItemId id) => Items.FirstOrDefault(it => it.Key.Equals(id)).Value;

        public bool Contains(ItemId id) => Items.Exists(it => it.Key.Equals(id));
    }
}