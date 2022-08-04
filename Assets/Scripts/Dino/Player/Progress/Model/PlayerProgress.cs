
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Dino.Player.Progress.Model
{
    public class PlayerProgress
    {
        [JsonProperty]
        private readonly Dictionary<string, int> _passCount = new Dictionary<string, int>();
        [JsonProperty] public int CraftCount { get; private set; }
        [JsonProperty] public int LootCount { get; private set; }

        public int GameCount { get; set; }
        public int WinCount { get; set; }
        public int LoseCount => GameCount - WinCount;
        public int LevelNumber => WinCount;
        public int Kills { get; set; }

        public static PlayerProgress Create() => new PlayerProgress();
 
        public int GetPassCount(string levelId) => _passCount.ContainsKey(levelId) ? _passCount[levelId] : 0;

        public void IncreasePassCount(string levelId)
        {
            _passCount[levelId] = GetPassCount(levelId) + 1;
        }

        public void IncreaseCraftCount()
        {
            CraftCount++;
        }

        public void IncreaseLootCount()
        {
            LootCount++;
        }
    }
}