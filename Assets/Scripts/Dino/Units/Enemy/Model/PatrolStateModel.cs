using Dino.Units.Enemy.Config;

namespace Dino.Units.Enemy.Model
{
    public class PatrolStateModel
    {
        public PatrolStateModel(PatrolStateConfig config)
        {
            FieldOfViewAngle = config.FieldOfViewAngle;
            FieldOfViewDistance = config.FieldOfViewDistance;
            PatrolIdleTime = config.PatrolIdleTime;
        }
        
        public float FieldOfViewAngle { get; }
        public float FieldOfViewDistance { get; }
        public float PatrolIdleTime { get; }
    }
}