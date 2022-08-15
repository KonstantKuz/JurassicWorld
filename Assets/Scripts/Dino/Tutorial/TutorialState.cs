using System.Collections.Generic;
using Newtonsoft.Json;

namespace Dino.Tutorial
{
    public class TutorialState
    {
        [JsonProperty]
        public readonly List<string> CompletedStages = new List<string>();
    }
}