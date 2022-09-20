using System.Runtime.Serialization;

namespace Dino.Units.Enemy.Config
{
    [DataContract]
    public class EnemyAttackConfig
    {
        [DataMember] 
        public AttackVariant AttackVariant;
        [DataMember] 
        public RegularAttackConfig RegularAttackConfig;
        [DataMember]
        public BulldozingAttackConfig BulldozingAttackConfig;
        [DataMember]
        public JumpingAttackConfig JumpingAttackConfig;
    }
}