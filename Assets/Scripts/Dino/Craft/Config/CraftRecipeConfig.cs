using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Dino.Craft.Config
{
    public class CraftRecipeConfig
    {
        public string CraftItemId { get; }
        public IReadOnlyList<string> Ingredients { get; }
        
        public CraftRecipeConfig(string craftItemId, IEnumerable<IngredientConfig> ingredients)
        {
            CraftItemId = craftItemId;
            Ingredients = ingredients.Select(it => it.Ingredient).ToList();
        }
    }
    
    [DataContract]
    public class IngredientConfig
    {
        [DataMember(Name = "Ingredient")]
        public string Ingredient;
    }
}