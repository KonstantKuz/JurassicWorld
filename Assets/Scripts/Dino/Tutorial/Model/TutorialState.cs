using System.Collections.Generic;
using Dino.Tutorial.Scenario;

namespace Dino.Tutorial.Model
{
    public class TutorialState
    {
        public readonly Dictionary<TutorialScenarioId, ScenarioState> Scenarios = new Dictionary<TutorialScenarioId, ScenarioState>();
    }
}