using Feofun.DroppingLoot.Message;
using UnityEngine;

namespace Feofun.DroppingLoot.Model
{
    public class DroppingLootInitParams
    {
        public DroppingLootType Type { get; }
        public int Count { get; }
        public string IconPath { get; }
        public Vector2 StartPosition { get; }
        public Vector2 FinishPosition { get; }

        public DroppingLootInitParams(DroppingLootType type, int count, string iconPath, Vector2 startPosition, Vector2 finishPosition)
        {
            Type = type;
            Count = count;
            IconPath = iconPath;
            StartPosition = startPosition;
            FinishPosition = finishPosition;
        }
        public static DroppingLootInitParams FromReceivedMessage(UILootReceivedMessage msg, Vector2 finishPosition)
        {
            return new DroppingLootInitParams(msg.Type, msg.Count, msg.IconPath, msg.StartPosition, finishPosition);
        }
    }
}