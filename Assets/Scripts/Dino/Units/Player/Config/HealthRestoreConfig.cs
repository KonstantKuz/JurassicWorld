using System.Runtime.Serialization;

namespace Dino.Units.Player.Config
{
    [DataContract]
    public class HealthRestoreConfig
    {
        [DataMember]
        public float TimeoutBeforeRecoverHealth;
        [DataMember]
        public float HealthRecoveryPeriod;
        [DataMember]
        public float HealthRecoveryValue;
    }
}