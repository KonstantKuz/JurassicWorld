using System.Runtime.Serialization;

namespace Dino.Location.Level.Config
{
    [DataContract]
    public class LevelConfig
    {
        [DataMember(Name = "LevelId")]
        public string LevelId;
    }
}