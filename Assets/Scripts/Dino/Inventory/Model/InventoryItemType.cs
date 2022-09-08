namespace Dino.Inventory.Model
{
    public enum InventoryItemType
    {
        Weapon, 
        Material,
        Ammo
    }

    public static class InventoryItemTypeExt
    {
        public static bool IsEquipable(this InventoryItemType type) => type == InventoryItemType.Weapon;
    }
}