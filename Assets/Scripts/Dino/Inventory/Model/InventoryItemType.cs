namespace Dino.Inventory.Model
{
    public enum InventoryItemType
    {
        Weapon, 
        Material
    }

    public static class InventoryItemTypeExt
    {
        public static bool IsEquipable(this InventoryItemType type) => type == InventoryItemType.Weapon;
    }
}