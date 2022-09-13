namespace Dino.Inventory.Model
{
    public readonly struct ItemChangedEvent
    {
        public readonly ItemId ItemId;   
        public readonly int PreviousAmount;   
        public readonly int CurrentAmount;

        public bool IsItemRemoved => CurrentAmount == 0 && PreviousAmount != 0;      
        public bool IsItemAddedAsNew => PreviousAmount == 0 && CurrentAmount != 0;
        
        public ItemChangedEvent(ItemId itemId, int previousAmount, int currentAmount)
        {
            ItemId = itemId;
            PreviousAmount = previousAmount;
            CurrentAmount = currentAmount;
        }
    }
}