using UnityEngine;

namespace Feofun.ReceivingLoot.Model
{
    public class FlyingIconTrajectory
    {
        public Vector2 StartPosition;
        public Vector2 RemovePosition;
        public float Height;
        public float Time;

        public FlyingIconTrajectory(Vector2 startPosition, Vector2 removePosition, float height, float time)
        {
            StartPosition = startPosition;
            RemovePosition = removePosition;
            Height = height;
            Time = time;
        }
        public static FlyingIconTrajectory FromFlyingIconViewModel(FlyingIconViewModel viewModel)
        {
            return new FlyingIconTrajectory(viewModel.StartPosition, viewModel.RemovePosition, viewModel.TrajectoryHeight, viewModel.Duration);
        }
    }
}