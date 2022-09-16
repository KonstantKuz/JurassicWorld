using System.Collections.Generic;
using System.IO;
using System.Linq;
using Feofun.Config;
using Feofun.Config.Csv;

namespace Dino.Inventory.Config
{
    public class InitialInventoryConfig : ILoadableConfig
    {
        private Dictionary<string, IReadOnlyList<ProvidedItemConfig>> _itemsMap;
        public void Load(Stream stream)
        {
            _itemsMap = new CsvSerializer().ReadNestedTable<ProvidedItemConfig>(stream)
                .ToDictionary(it => it.Key, it => it.Value);
        }

        public IReadOnlyList<ProvidedItemConfig> FindProvidedItems(string levelId)
        {
            return _itemsMap.ContainsKey(levelId) ? _itemsMap[levelId] : null;
        }
    }
}