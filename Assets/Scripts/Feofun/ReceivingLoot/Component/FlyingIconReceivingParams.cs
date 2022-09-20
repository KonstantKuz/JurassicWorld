using UnityEngine;

namespace Feofun.ReceivingLoot.Component
{
    public struct FlyingIconReceivingParams
    {
        public string Type { get; }
        public string IconPath { get; }
        public int Count { get; }
        public Vector2 StartPosition { get; }

        public FlyingIconReceivingParams(string type, string iconPath, int count, Vector2 startPosition)
        {
            Type = type;
            Count = count;
            StartPosition = startPosition;
            IconPath = iconPath;
        }

        public static FlyingIconReceivingParams Create(string type, string iconPath, int count, Vector2 startPosition)
        {
            return new FlyingIconReceivingParams(type, iconPath, count, startPosition);
        }
    }
}