using Dino.Units.Component.Health;
using Feofun.Components;

namespace Dino.Squad.Component
{
    public class SquadWithHealth : Health, IInitializable<Squad>
    {
        public void Init(Squad squad)
        {
            base.Init(squad.Model.HealthModel);
        }
    }
}