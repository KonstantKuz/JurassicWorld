﻿using UnityEngine;

namespace Feofun.DroppingLoot.Model
{
    public class DroppingObjectTrajectory
    {
        public Vector2 StartPosition;
        public Vector2 RemovePosition;
        public float Height;
        public float Time;

        public DroppingObjectTrajectory(Vector2 startPosition, Vector2 removePosition, float height, float time)
        {
            StartPosition = startPosition;
            RemovePosition = removePosition;
            Height = height;
            Time = time;
        }
        public static DroppingObjectTrajectory FromDroppingObject(DroppingObjectViewModel viewModel)
        {
            return new DroppingObjectTrajectory(viewModel.StartPosition, viewModel.RemovePosition, viewModel.TrajectoryHeight, viewModel.Duration);
        }
    }
}