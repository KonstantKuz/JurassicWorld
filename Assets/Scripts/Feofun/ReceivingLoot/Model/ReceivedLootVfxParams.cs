using Feofun.ReceivingLoot.Message;
using UnityEngine;

namespace Feofun.ReceivingLoot.Model
{
    public class ReceivedLootVfxParams
    {
        public int Count { get; }
        public string IconPath { get; }
        public Vector2 StartPosition { get; }
        public Vector2 FinishPosition { get; }

        public ReceivedLootVfxParams(int count, string iconPath, Vector2 startPosition, Vector2 finishPosition)
        {
            Count = count;
            IconPath = iconPath;
            StartPosition = startPosition;
            FinishPosition = finishPosition;
        }

        public static ReceivedLootVfxParams FromReceivedMessage(UILootReceivedMessage msg, Vector2 finishPosition)
        {
            return new ReceivedLootVfxParams(msg.Count, msg.IconPath, msg.StartPosition, finishPosition);
        }
    }
}