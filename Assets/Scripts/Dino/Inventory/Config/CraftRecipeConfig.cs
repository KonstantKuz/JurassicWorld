using System.Collections.Generic;
using System.Linq;

namespace Dino.Inventory.Config
{
    public class CraftRecipeConfig
    {
        public string CraftItemId { get; }
        public IReadOnlyList<IngredientConfig> Ingredients { get; }
        
        public CraftRecipeConfig(string craftItemId, IEnumerable<IngredientConfig> ingredients)
        {
            CraftItemId = craftItemId;
            Ingredients = ingredients.ToList();
        }

        public bool ContainsIngredient(string itemName)
        {
            return Ingredients.Select(it => it.Name).Contains(itemName);
        }
    }
}