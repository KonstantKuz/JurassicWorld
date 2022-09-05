using System.Collections.Generic;

namespace Dino.Tutorial
{
    public class TutorialState
    {
        public readonly Dictionary<TutorialScenarioId, ScenarioState> Scenarios = new Dictionary<TutorialScenarioId, ScenarioState>();
    }
}