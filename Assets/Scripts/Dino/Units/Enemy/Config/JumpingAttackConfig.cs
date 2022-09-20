using System.Runtime.Serialization;

namespace Dino.Units.Enemy.Config
{
    [DataContract]
    public class JumpingAttackConfig
    {
        [DataMember(Name = "JumpingDuration")] 
        public float Duration;
        [DataMember(Name = "JumpingHeight")] 
        public float Height;
        [DataMember(Name = "JumpingDamageRadius")]
        public float DamageRadius;
        [DataMember(Name = "JumpingSafeTime")]
        public float PlayerSafeTime;
    }
}