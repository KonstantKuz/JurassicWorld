using UnityEngine;

namespace Feofun.ReceivingLoot.Model
{
    public class ReceivedLootTrajectory
    {
        public Vector2 StartPosition;
        public Vector2 RemovePosition;
        public float Height;
        public float Time;

        public ReceivedLootTrajectory(Vector2 startPosition, Vector2 removePosition, float height, float time)
        {
            StartPosition = startPosition;
            RemovePosition = removePosition;
            Height = height;
            Time = time;
        }
        public static ReceivedLootTrajectory FromReceivedLootModel(ReceivedLootViewModel viewModel)
        {
            return new ReceivedLootTrajectory(viewModel.StartPosition, viewModel.RemovePosition, viewModel.TrajectoryHeight, viewModel.Duration);
        }
    }
}