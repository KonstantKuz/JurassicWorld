using DinoWorldSurvival.Cheats.Data;
using Feofun.Repository;

namespace DinoWorldSurvival.Cheats.Repository
{
    public class CheatRepository : LocalPrefsSingleRepository<CheatSettings>
    {
        public CheatRepository() : base("CheatSettings")
        {
        }
    }
}