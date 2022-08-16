using System.Collections.Generic;
using Dino.Util;
using Newtonsoft.Json;

namespace Dino.Tutorial
{
    public class TutorialState
    {
        [JsonConverter(typeof(CustomHashSetConverter<string>))]
        public HashSet<string> CompletedStages = new HashSet<string>();
    }
}