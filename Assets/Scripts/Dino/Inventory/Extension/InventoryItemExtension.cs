using Dino.Inventory.Model;
using Feofun.ReceivingLoot.Message;
using SuperMaxim.Messaging;
using UnityEngine;

namespace Dino.Inventory.Extension
{
    public static class InventoryItemExtension
    { 
        public static FlyingIconVfxReceivedMessage ToLootReceivedMessage(this Item item, int count, Vector2 startPosition)
        {
            var iconPath = Util.IconPath.GetInventory(item.Name);
            return FlyingIconVfxReceivedMessage.Create(item.Type.ToString(), iconPath, count, startPosition);
        }
    }
}