using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using Feofun.Config;
using Feofun.Config.Csv;

namespace Dino.Craft.Config
{
    [DataContract]
    public class CraftConfig : ILoadableConfig
    {
        public Dictionary<string, CraftRecipeConfig> Crafts { get; private set; }

        public CraftRecipeConfig GetRecipe(string craftItemId)
        {
            if (Crafts.ContainsKey(craftItemId)) {
                throw new NullReferenceException($"CraftRecipeConfig is null by id:= {craftItemId}");
            }
            return Crafts[craftItemId];
        }
        public void Load(Stream stream)
        {
            var parsed = new CsvSerializer().ReadNestedTable<IngredientConfig>(stream);
            Crafts = parsed.ToDictionary(it => it.Key, it => new CraftRecipeConfig(it.Key, it.Value));
        }
        
    

    }
}