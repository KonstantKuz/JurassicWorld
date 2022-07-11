using System.Runtime.Serialization;
using Feofun.Config;

namespace Dino.Squad.Config
{
    [DataContract]
    public class SquadLevelConfig : ICollectionItem<string>
    {
        [DataMember]
        public int Level;
        [DataMember]
        public int ExpToNextLevel;
        
        public string Id => Level.ToString();
    }
}