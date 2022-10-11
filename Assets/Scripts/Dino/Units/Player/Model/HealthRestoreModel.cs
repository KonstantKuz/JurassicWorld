using Dino.Units.Player.Config;

namespace Dino.Units.Player.Model
{
    public class HealthRestoreModel
    {
        public float TimeoutBeforeRecover { get; }
        public float RecoveryPeriod { get; }
        public float RecoveryValue { get; }
        public HealthRestoreModel(HealthRestoreConfig config)
        {
            TimeoutBeforeRecover = config.TimeoutBeforeRecoverHealth;
            RecoveryPeriod = config.HealthRecoveryPeriod;
            RecoveryValue = config.HealthRecoveryValue;
        }
    }
}