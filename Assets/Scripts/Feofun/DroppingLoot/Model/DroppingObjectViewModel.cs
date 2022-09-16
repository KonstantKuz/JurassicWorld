using Feofun.DroppingLoot.Config;
using UnityEngine;

namespace Feofun.DroppingLoot.Model
{
    public class DroppingObjectViewModel
    {
        public float Duration { get; }
        public float TrajectoryHeight { get; }
        public string Icon { get; }
        public Vector2 StartPosition { get; }
        public Vector2 RemovePosition { get; }
        public DroppingLootConfig Config { get; }

        public DroppingObjectViewModel(DroppingLootInitParams initParams, DroppingLootConfig config, Vector2 startPosition)
        {
            Config = config;
            Duration = Config.DroppingTime + Random.Range(-Config.DroppingTimeDispersion, Config.DroppingTimeDispersion);
            TrajectoryHeight = Random.Range(Config.MinTrajectoryHeight, Config.MaxTrajectoryHeight);
            Icon = initParams.IconPath;
            StartPosition = startPosition;
            RemovePosition = initParams.FinishPosition;
        }

        public static DroppingObjectViewModel Create(DroppingLootInitParams initParams, DroppingLootConfig config, Vector2 startPosition)
        {
            return new DroppingObjectViewModel(initParams, config, startPosition);
        }
    }
}