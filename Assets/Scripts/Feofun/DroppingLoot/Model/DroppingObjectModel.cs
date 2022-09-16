using Dino.Util;
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

        public DroppingObjectModel(DroppingLootModel lootModel, DroppingLootConfig config, Vector2 startPosition)
        {
            Config = config;
            DroppingTime = Config.DroppingTime + Random.Range(-Config.DroppingTimeDispersion, Config.DroppingTimeDispersion);
            DroppingTrajectoryHeight = Random.Range(Config.MinTrajectoryHeight, Config.MaxTrajectoryHeight);
            Icon = IconPath.GetInventory(lootModel.LootId);
            StartPosition = startPosition;
            RemovePosition = lootModel.FinishPosition;
        }

        public static DroppingObjectModel Create(DroppingLootModel lootModel, DroppingLootConfig config, Vector2 startPosition)
        {
            return new DroppingObjectModel(lootModel, config, startPosition);
        }
    }
}