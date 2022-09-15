using Feofun.DroppingLoot.Config;
using UnityEngine;

namespace Feofun.DroppingLoot.Model
{
    public class DroppingObjectModel
    {
        public float DroppingTime { get; }
        public float DroppingTrajectoryHeight { get; }
        public string Icon { get; }
        public Vector2 StartPosition { get; }
        public Vector2 RemovePosition { get; }
        public DroppingLootConfig Config { get; }

        public DroppingObjectModel(DroppingLootType lootType, DroppingLootConfig config, Vector2 startPosition, Vector2 removePosition)
        {
            Config = config;
            DroppingTime = Config.DroppingTime + Random.Range(-Config.DroppingTimeDispersion, Config.DroppingTimeDispersion);
            DroppingTrajectoryHeight = Random.Range(Config.MinTrajectoryHeight, Config.MaxTrajectoryHeight);
            Icon = IconPath.GetDroppingLoot(lootType.ToString());
            StartPosition = startPosition;
            RemovePosition = removePosition;
        }

        public static DroppingObjectModel Create(DroppingLootType lootType, DroppingLootConfig config, Vector2 startPosition, Vector2 removePosition)
        {
            return new DroppingObjectModel(lootType, config, startPosition, removePosition);
        }
    }
}