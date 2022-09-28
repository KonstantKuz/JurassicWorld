using System.Runtime.Serialization;

namespace Dino.Units.Enemy.Config
{
    [DataContract]
    public class EnemyAttacksConfig
    {
        [DataMember]
        public BulldozingAttackConfig BulldozingAttackConfig;
        [DataMember]
        public JumpingAttackConfig JumpingAttackConfig;
    }
}