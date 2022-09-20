using UnityEngine;

namespace Feofun.ReceivingLoot.Message
{
    public struct FlyingIconVfxReceivedMessage
    {
        public string Type { get; }
        public string IconPath { get; }
        public int Count { get; }
        public Vector2 StartPosition { get; }

        public FlyingIconVfxReceivedMessage(string type, string iconPath, int count, Vector2 startPosition)
        {
            Type = type;
            Count = count;
            StartPosition = startPosition;
            IconPath = iconPath;
        }

        public static FlyingIconVfxReceivedMessage Create(string type, string iconPath, int count, Vector2 startPosition)
        {
            return new FlyingIconVfxReceivedMessage(type, iconPath, count, startPosition);
        }
    }
}