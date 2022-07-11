using DinoWorldSurvival.Units.Component.Health;
using Feofun.Components;

namespace DinoWorldSurvival.Squad.Component
{
    public class SquadWithHealth : Health, IInitializable<Squad>
    {
        public void Init(Squad squad)
        {
            base.Init(squad.Model.HealthModel);
        }
    }
}