using Dino.Inventory.Model;
using Feofun.DroppingLoot.Message;
using Feofun.DroppingLoot.Model;
using SuperMaxim.Messaging;
using UnityEngine;

namespace Dino.Inventory.Extension
{
    public static class InventoryItemExtension
    { 
        public static void TryPublishReceivedLoot(this Item item, IMessenger messenger, int count, Vector2 startPosition)
        {
            var droppingLootType = DroppingLootTypeExt.ValueOf(item.Type.ToString());
            var iconPath = Util.IconPath.GetInventory(item.Name);
            var message = UILootReceivedMessage.Create(droppingLootType, iconPath, count, startPosition);
            messenger.Publish(message);
        }
    }
}