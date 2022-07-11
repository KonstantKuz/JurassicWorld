using System.Runtime.Serialization;

namespace DinoWorldSurvival.Squad.Config
{
    public class SquadConfig
    {
        [DataMember] 
        public float Speed;
        [DataMember]
        public float CollectRadius;
    }
}