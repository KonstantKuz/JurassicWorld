using DinoWorldSurvival.Player.Progress.Model;
using Feofun.Repository;

namespace DinoWorldSurvival.Player.Progress.Service
{
    public class PlayerProgressRepository : LocalPrefsSingleRepository<PlayerProgress>
    {
        protected PlayerProgressRepository() : base("PlayerProgress_v1")
        {
        }
    }
}