using Dino.Inventory.Model;
using Feofun.DroppingLoot.Message;
using SuperMaxim.Messaging;
using UnityEngine;

namespace Dino.Inventory.Extension
{
    public static class InventoryItemExtension
    { 
        public static void TryPublishReceivedLoot(this Item item, IMessenger messenger, int count, Vector2 startPosition)
        {
            var iconPath = Util.IconPath.GetInventory(item.Name);
            var message = UILootReceivedMessage.Create(item.Type.ToString(), iconPath, count, startPosition);
            messenger.Publish(message);
        }
    }
}