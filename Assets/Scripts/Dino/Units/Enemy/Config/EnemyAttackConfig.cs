using System.Runtime.Serialization;

namespace Dino.Units.Enemy.Config
{
    [DataContract]
    public class EnemyAttackConfig
    {
        [DataMember]
        public float AttackRange;
        [DataMember]
        public int AttackDamage;
        [DataMember]
        public float AttackInterval;
    }
}