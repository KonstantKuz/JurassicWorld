using System.Collections.Generic;
using System.IO;
using System.Linq;
using Feofun.Config;
using Feofun.Config.Csv;

namespace Dino.Inventory.Config
{
    public class ItemProviderConfig : ILoadableConfig
    {
        private Dictionary<string, IReadOnlyList<ProvidedItemConfig>> ItemsMap { get; set; }
        public void Load(Stream stream)
        {
            ItemsMap = new CsvSerializer().ReadNestedTable<ProvidedItemConfig>(stream)
                .ToDictionary(it => it.Key, it => it.Value);
        }

        public IReadOnlyList<ProvidedItemConfig> GetProvidedItems(string levelId)
        {
            return ItemsMap.ContainsKey(levelId) ? ItemsMap[levelId] : null;
        }
    }
}