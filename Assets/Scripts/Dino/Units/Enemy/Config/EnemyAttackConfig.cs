using System.Runtime.Serialization;

namespace Dino.Units.Enemy.Config
{
    [DataContract]
    public class EnemyAttackConfig
    {
        [DataMember]
        public float AttackDistance;
        [DataMember]
        public int AttackDamage;
        [DataMember]
        public float AttackInterval;
    }
}