using Feofun.DroppingLoot.Message;
using UnityEngine;

namespace Feofun.DroppingLoot.Model
{
    public class DroppingLootInitParams
    {
        public int Count { get; }
        public string IconPath { get; }
        public Vector2 StartPosition { get; }
        public Vector2 FinishPosition { get; }

        public DroppingLootInitParams(int count, string iconPath, Vector2 startPosition, Vector2 finishPosition)
        {
            Count = count;
            IconPath = iconPath;
            StartPosition = startPosition;
            FinishPosition = finishPosition;
        }

        public static DroppingLootInitParams FromReceivedMessage(UILootReceivedMessage msg, Vector2 finishPosition)
        {
            return new DroppingLootInitParams(msg.Count, msg.IconPath, msg.StartPosition, finishPosition);
        }
    }
}