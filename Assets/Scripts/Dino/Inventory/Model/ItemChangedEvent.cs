namespace Dino.Inventory.Model
{
    public readonly struct ItemChangedEvent
    {
        public readonly ItemId ItemId;
        public readonly InventoryItemType Type;
        public readonly int PreviousAmount;   
        public readonly int CurrentAmount;

        public bool IsItemDecreased => CurrentAmount < PreviousAmount;   
        public bool IsItemRemoved => CurrentAmount == 0 && PreviousAmount != 0;      
        public bool IsItemAddedAsNew => PreviousAmount == 0 && CurrentAmount != 0;
        
        public ItemChangedEvent(ItemId itemId, InventoryItemType type, int previousAmount, int currentAmount)
        {
            ItemId = itemId;
            PreviousAmount = previousAmount;
            CurrentAmount = currentAmount;
            Type = type;
        }
    }
}