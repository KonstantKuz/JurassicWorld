using Dino.Units.Enemy.Config;

namespace Dino.Units.Enemy.Model
{
    public class LookAroundStateModel
    {
        public LookAroundStateModel(LookAroundStateConfig config)
        {
            LookAroundTime = config.LookAroundTime;
            LookAroundSpeed = config.LookAroundSpeed;
        }
        
        public float LookAroundTime { get; }
        public float LookAroundSpeed { get; }
    }
}