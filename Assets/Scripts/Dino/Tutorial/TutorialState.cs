using System.Collections.Generic;
using Dino.Util;
using Newtonsoft.Json;

namespace Dino.Tutorial
{
    public class TutorialState
    {
        public readonly Dictionary<TutorialScenarioId, ScenarioState> Scenarios = new Dictionary<TutorialScenarioId, ScenarioState>();
    }
    
    public class ScenarioState
    {
        public bool IsCompleted;
        [JsonConverter(typeof(CustomHashSetConverter<string>))]
        public HashSet<string> CompletedSteps = new HashSet<string>();
    }
}