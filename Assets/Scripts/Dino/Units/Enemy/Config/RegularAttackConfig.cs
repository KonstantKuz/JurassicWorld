using System.Runtime.Serialization;

namespace Dino.Units.Enemy.Config
{
    [DataContract]
    public class RegularAttackConfig
    {
        [DataMember]
        public float AttackDamage;
        [DataMember]
        public float AttackDistance;
        [DataMember]
        public float AttackInterval;
    }
}