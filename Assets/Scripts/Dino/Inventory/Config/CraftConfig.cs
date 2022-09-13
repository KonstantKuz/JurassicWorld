using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using Feofun.Config;
using Feofun.Config.Csv;

namespace Dino.Inventory.Config
{
    [DataContract]
    public class CraftConfig : ILoadableConfig
    {
        public Dictionary<string, CraftRecipeConfig> Crafts { get; private set; }

        public CraftRecipeConfig GetRecipe(string recipeId)
        {
            if (!Crafts.ContainsKey(recipeId)) {
                throw new NullReferenceException($"CraftRecipeConfig is null by id:= {recipeId}");
            }
            return Crafts[recipeId];
        }
        public void Load(Stream stream)
        {
            Crafts = new CsvSerializer().ReadObjectAndNestedTable<CraftItemConfig, IngredientConfig>(stream)
                                        .ToDictionary(it => it.Key,
                                                      it => CreateRecipeConfig(it.Key, it.Value.Item1, it.Value.Item2));
        }

        private CraftRecipeConfig CreateRecipeConfig(string craftItemId, CraftItemConfig craftItemConfig, IReadOnlyList<IngredientConfig> ingredients)
        {
            var craftItemConfigWithId = CraftItemConfig.Create(craftItemId, craftItemConfig);
            return new CraftRecipeConfig(craftItemConfigWithId, ingredients);
        }
    }
}