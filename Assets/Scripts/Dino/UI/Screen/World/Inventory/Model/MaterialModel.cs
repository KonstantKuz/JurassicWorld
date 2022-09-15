using Dino.Inventory.Model;

namespace Dino.UI.Screen.World.Inventory.Model
{
    public class MaterialViewModel
    {
        public readonly string Id;
        public readonly int Amount;
        
        public MaterialViewModel(Item item)
        {
            Id = item.Name;
            Amount = item.Amount;
        }
    }
}
