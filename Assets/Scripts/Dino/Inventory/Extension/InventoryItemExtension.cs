using Dino.Inventory.Model;
using Feofun.ReceivingLoot.Component;
using SuperMaxim.Messaging;
using UnityEngine;

namespace Dino.Inventory.Extension
{
    public static class InventoryItemExtension
    { 
        public static FlyingIconReceivingParams ToFlyingIconReceivingParams(this Item item, int count, Vector2 startPosition)
        {
            var iconPath = Util.IconPath.GetInventory(item.Name);
            return FlyingIconReceivingParams.Create(item.Type.ToString(), iconPath, count, startPosition);
        }
    }
}