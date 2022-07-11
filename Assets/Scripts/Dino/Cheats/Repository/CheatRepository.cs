using Dino.Cheats.Data;
using Feofun.Repository;

namespace Dino.Cheats.Repository
{
    public class CheatRepository : LocalPrefsSingleRepository<CheatSettings>
    {
        public CheatRepository() : base("CheatSettings")
        {
        }
    }
}