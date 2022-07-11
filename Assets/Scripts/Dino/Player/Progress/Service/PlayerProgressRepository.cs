using Dino.Player.Progress.Model;
using Feofun.Repository;

namespace Dino.Player.Progress.Service
{
    public class PlayerProgressRepository : LocalPrefsSingleRepository<PlayerProgress>
    {
        protected PlayerProgressRepository() : base("PlayerProgress_v1")
        {
        }
    }
}