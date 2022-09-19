using Dino.Inventory.Model;
using Feofun.ReceivingLoot.Message;
using SuperMaxim.Messaging;
using UnityEngine;

namespace Dino.Inventory.Extension
{
    public static class InventoryItemExtension
    { 
        public static UILootReceivedMessage ToLootReceivedMessage(this Item item, int count, Vector2 startPosition)
        {
            var iconPath = Util.IconPath.GetInventory(item.Name);
            return UILootReceivedMessage.Create(item.Type.ToString(), iconPath, count, startPosition);
        }
    }
}