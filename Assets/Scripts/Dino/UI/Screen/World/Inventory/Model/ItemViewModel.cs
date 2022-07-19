using System;
using Dino.Inventory.Model;
using JetBrains.Annotations;

namespace Dino.UI.Screen.World.Inventory.Model
{
    public class ItemViewModel
    {
        
        [CanBeNull]
        public InventoryItem Item { get; set; }
        public ItemViewState State { get; set; }
        public Action OnClick { get; set; }
        
        public static ItemViewModel Empty()
        {
            return new ItemViewModel() {
                    State = ItemViewState.Empty
            };
        }
    }

    public enum ItemViewState
    {
        Active,
        Inactive,
        Empty,
    }
}