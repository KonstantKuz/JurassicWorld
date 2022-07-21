using System;
using Dino.Inventory.Model;
using JetBrains.Annotations;

namespace Dino.UI.Screen.World.Inventory.Model
{
    public class ItemViewModel
    {
        
        [CanBeNull]
        public ItemId Id { get; set; }
        public ItemViewState State { get; set; }
        public Action OnClick { get; set; }    
        
        public Action<ItemViewModel> OnBeginDrag { get; set; }    
        public Action<ItemViewModel> OnEndDrag { get; set; }
        
        public static ItemViewModel Empty()
        {
            return new ItemViewModel() {
                    State = ItemViewState.Empty
            };
        }
    }
}