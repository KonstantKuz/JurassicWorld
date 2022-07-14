using System.Runtime.Serialization;

namespace Dino.Units.Enemy.Config
{
    [DataContract]
    public class EnemyBehaviourConfig
    {
        [DataMember] 
        public float FieldOfViewAngle;
        [DataMember] 
        public float FieldOfViewDistance;
        [DataMember] 
        public float PatrolIdleTime;
    }
}