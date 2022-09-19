using UnityEngine;

namespace Feofun.ReceivingLoot.Message
{
    public struct UILootReceivedMessage
    {
        public string Type { get; }
        public string IconPath { get; }
        public int Count { get; }
        public Vector2 StartPosition { get; }

        public UILootReceivedMessage(string type, string iconPath, int count, Vector2 startPosition)
        {
            Type = type;
            Count = count;
            StartPosition = startPosition;
            IconPath = iconPath;
        }

        public static UILootReceivedMessage Create(string type, string iconPath, int count, Vector2 startPosition)
        {
            return new UILootReceivedMessage(type, iconPath, count, startPosition);
        }
    }
}