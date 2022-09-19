using Feofun.ReceivingLoot.Message;
using UnityEngine;

namespace Feofun.ReceivingLoot.Model
{
    public class ReceivedLootInitParams
    {
        public int Count { get; }
        public string IconPath { get; }
        public Vector2 StartPosition { get; }
        public Vector2 FinishPosition { get; }

        public ReceivedLootInitParams(int count, string iconPath, Vector2 startPosition, Vector2 finishPosition)
        {
            Count = count;
            IconPath = iconPath;
            StartPosition = startPosition;
            FinishPosition = finishPosition;
        }

        public static ReceivedLootInitParams FromReceivedMessage(UILootReceivedMessage msg, Vector2 finishPosition)
        {
            return new ReceivedLootInitParams(msg.Count, msg.IconPath, msg.StartPosition, finishPosition);
        }
    }
}