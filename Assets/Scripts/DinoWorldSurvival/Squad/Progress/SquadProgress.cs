using System;
using DinoWorldSurvival.Squad.Config;
using Feofun.Config;

namespace DinoWorldSurvival.Squad.Progress
{
    public class SquadProgress
    {
        public const int DEFAULT_LEVEL = 1;
        
        public int Level = DEFAULT_LEVEL;
        public int Exp;
        public static SquadProgress Create() => new SquadProgress();
        public bool IsMaxLevel(StringKeyedConfigCollection<SquadLevelConfig> levels) => Level > levels.Values.Count;

        public int MaxExpForCurrentLevel(StringKeyedConfigCollection<SquadLevelConfig> levels) => CurrentLevelConfig(levels).ExpToNextLevel;

        public SquadLevelConfig CurrentLevelConfig(StringKeyedConfigCollection<SquadLevelConfig> levels) =>
                levels.Values[Math.Min(levels.Values.Count - 1, Level - 1)];

        public void AddExp(int amount, StringKeyedConfigCollection<SquadLevelConfig> levels)
        {
            Exp += amount;
            while (Exp >= MaxExpForCurrentLevel(levels) && !IsMaxLevel(levels))
            {
                Exp -= MaxExpForCurrentLevel(levels);                
                Level++;
            }
        }
    }
}