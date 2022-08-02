using System.Runtime.Serialization;

namespace Dino.Units.Enemy.Config
{
    [DataContract]
    public class RankParamsConfig
    {
        [DataMember] 
        public int HealthStep;
        [DataMember]
        public int DamageStep;
    }
}