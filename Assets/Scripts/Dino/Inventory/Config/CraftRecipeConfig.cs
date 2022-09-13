using System.Collections.Generic;
using System.Linq;

namespace Dino.Inventory.Config
{
    public class CraftRecipeConfig
    {
        public CraftItemConfig CraftItem { get; }
        public IReadOnlyList<IngredientConfig> Ingredients { get; }

        public string CraftItemId => CraftItem.Id;

        public CraftRecipeConfig(CraftItemConfig craftItemConfig, IEnumerable<IngredientConfig> ingredients)
        {
            CraftItem = craftItemConfig;
            Ingredients = ingredients.ToList();
        }

        public bool ContainsIngredient(string ingredientId)
        {
            return Ingredients.Select(it => it.Id).Contains(ingredientId);
        }
    }
}