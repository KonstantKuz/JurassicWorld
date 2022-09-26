using Feofun.ReceivingLoot.Config;
using UnityEngine;

namespace Feofun.ReceivingLoot.Model
{
    public class FlyingIconViewModel
    {
        public float Duration { get; }
        public float TrajectoryHeight { get; }
        public string Icon { get; }
        public Vector2 StartPosition { get; }
        public Vector2 RemovePosition { get; }
        public FlyingIconVfxConfig VfxConfig { get; }

        public FlyingIconViewModel(FlyingIconVfxParams vfxParams, FlyingIconVfxConfig vfxConfig, Vector2 startPosition)
        {
            VfxConfig = vfxConfig;
            Duration = VfxConfig.ReceivedTime + Random.Range(-VfxConfig.ReceivedTimeDispersion, VfxConfig.ReceivedTimeDispersion);
            TrajectoryHeight = Random.Range(VfxConfig.MinTrajectoryHeight, VfxConfig.MaxTrajectoryHeight);
            Icon = vfxParams.IconPath;
            StartPosition = startPosition;
            RemovePosition = vfxParams.FinishPosition;
        }

        public static FlyingIconViewModel Create(FlyingIconVfxParams vfxParams, FlyingIconVfxConfig vfxConfig, Vector2 startPosition)
        {
            return new FlyingIconViewModel(vfxParams, vfxConfig, startPosition);
        }
    }
}