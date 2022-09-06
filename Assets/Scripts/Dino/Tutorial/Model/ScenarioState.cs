using System.Collections.Generic;
using Dino.Util;
using Newtonsoft.Json;

namespace Dino.Tutorial.Model
{
    public class ScenarioState
    {
        public bool IsCompleted;
        [JsonConverter(typeof(CustomHashSetConverter<string>))]
        public HashSet<string> CompletedSteps = new HashSet<string>();
    }
}