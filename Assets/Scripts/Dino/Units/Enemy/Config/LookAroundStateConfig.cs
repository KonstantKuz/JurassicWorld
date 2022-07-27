using System.Runtime.Serialization;

namespace Dino.Units.Enemy.Config
{
    [DataContract]
    public class LookAroundStateConfig
    {
        [DataMember] 
        public float LookAroundTime;
        [DataMember]
        public float LookAroundSpeed;
    }
}