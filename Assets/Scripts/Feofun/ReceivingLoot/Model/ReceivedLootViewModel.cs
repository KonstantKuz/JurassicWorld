using Feofun.ReceivingLoot.Config;
using UnityEngine;

namespace Feofun.ReceivingLoot.Model
{
    public class ReceivedLootViewModel
    {
        public float Duration { get; }
        public float TrajectoryHeight { get; }
        public string Icon { get; }
        public Vector2 StartPosition { get; }
        public Vector2 RemovePosition { get; }
        public ReceivedLootVfxConfig VfxConfig { get; }

        public ReceivedLootViewModel(ReceivedLootVfxParams vfxParams, ReceivedLootVfxConfig vfxConfig, Vector2 startPosition)
        {
            VfxConfig = vfxConfig;
            Duration = VfxConfig.ReceivedTime + Random.Range(-VfxConfig.ReceivedTimeDispersion, VfxConfig.ReceivedTimeDispersion);
            TrajectoryHeight = Random.Range(VfxConfig.MinTrajectoryHeight, VfxConfig.MaxTrajectoryHeight);
            Icon = vfxParams.IconPath;
            StartPosition = startPosition;
            RemovePosition = vfxParams.FinishPosition;
        }

        public static ReceivedLootViewModel Create(ReceivedLootVfxParams vfxParams, ReceivedLootVfxConfig vfxConfig, Vector2 startPosition)
        {
            return new ReceivedLootViewModel(vfxParams, vfxConfig, startPosition);
        }
    }
}