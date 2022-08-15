using System.Collections.Generic;
using Newtonsoft.Json;

namespace Dino.Tutorial
{
    public class TutorialState
    {
        [JsonProperty]
        public readonly HashSet<string> CompletedStages = new HashSet<string>();
    }
}