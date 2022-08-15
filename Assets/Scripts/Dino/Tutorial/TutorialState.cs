using Dino.Util;
using Feofun.Util;
using Newtonsoft.Json;

namespace Dino.Tutorial
{
    public class TutorialState
    {
        [JsonProperty]
        public readonly SerializableHashSet<string> CompletedStages = new SerializableHashSet<string>();
    }
}