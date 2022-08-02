using System.Collections.Generic;
using System.IO;
using System.Linq;
using Feofun.Config;
using Feofun.Config.Csv;

namespace Dino.Location.Level.Config
{
    public class LevelsConfig : ILoadableConfig
    {
        private List<string> _levels;
        
        public IReadOnlyList<string> Levels => _levels;
        public void Load(Stream stream)
        {
            _levels = new CsvSerializer().ReadObjectArray<LevelConfig>(stream)
                                         .Select(it => it.LevelId)
                                         .ToList();
        }
    }
}