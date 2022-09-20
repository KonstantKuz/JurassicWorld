using System.Runtime.Serialization;

namespace Dino.Units.Enemy.Config
{
    [DataContract]
    public class BulldozingAttackConfig
    {
        [DataMember(Name = "BulldozingSpeed")] 
        public float Speed;
        [DataMember(Name = "BulldozingRotationSpeed")] 
        public float RotationSpeed;
    }
}