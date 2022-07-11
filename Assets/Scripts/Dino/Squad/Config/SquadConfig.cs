using System.Runtime.Serialization;

namespace Dino.Squad.Config
{
    public class SquadConfig
    {
        [DataMember] 
        public float Speed;
        [DataMember]
        public float CollectRadius;
    }
}