using Feofun.ReceivingLoot.Component;
using UnityEngine;

namespace Feofun.ReceivingLoot.Model
{
    public class FlyingIconVfxParams
    {
        public int Count { get; }
        public string IconPath { get; }
        public Vector2 StartPosition { get; }
        public Vector2 FinishPosition { get; }

        public FlyingIconVfxParams(int count, string iconPath, Vector2 startPosition, Vector2 finishPosition)
        {
            Count = count;
            IconPath = iconPath;
            StartPosition = startPosition;
            FinishPosition = finishPosition;
        }

        public static FlyingIconVfxParams FromReceivedMessage(FlyingIconReceivingParams msg, Vector2 finishPosition)
        {
            return new FlyingIconVfxParams(msg.Count, msg.IconPath, msg.StartPosition, finishPosition);
        }
    }
}