namespace Dino.UI.Screen.World.Inventory.Model
{
    public class InventoryItem
    {
        public string Id;
        
        public ItemState State
    }

    public enum ItemState
    {
        Active,
        Inactive,
    }
}