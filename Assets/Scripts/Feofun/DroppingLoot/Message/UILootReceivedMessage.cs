using Feofun.DroppingLoot.Model;
using UnityEngine;

namespace Feofun.DroppingLoot.Message
{
    public struct UILootReceivedMessage
    {
        public DroppingLootType Type { get; }
        public string IconPath { get; }
        public int Count { get; }
        public Vector2 StartPosition { get; }

        public UILootReceivedMessage(DroppingLootType type, string iconPath, int count, Vector2 startPosition)
        {
            Type = type;
            Count = count;
            StartPosition = startPosition;
            IconPath = iconPath;
        }

        public static UILootReceivedMessage Create(DroppingLootType type, string iconPath, int count, Vector2 startPosition)
        {
            return new UILootReceivedMessage(type, iconPath, count, startPosition);
        }
    }
}