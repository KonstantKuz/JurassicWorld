using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Dino.Craft.Config
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
    
    [DataContract]
    public class IngredientConfig
    {
        [DataMember(Name = "Ingredient")]
        public string Name;     
        [DataMember(Name = "Count")]
        public int Count;
    }
}