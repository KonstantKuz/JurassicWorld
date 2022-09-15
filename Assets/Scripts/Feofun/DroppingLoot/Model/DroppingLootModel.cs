using Feofun.DroppingLoot.Message;
using UnityEngine;

namespace Feofun.DroppingLoot.Model
{
    public class DroppingLootModel
    {
        public DroppingLootType Type { get; }
        public int Count { get; }
        public Vector2 StartPosition { get; }
        public Vector2 FinishPosition { get; }

        public DroppingLootModel(DroppingLootType type, int count, Vector2 startPosition, Vector2 finishPosition)
        {
            Type = type;
            Count = count;
            StartPosition = startPosition;
            FinishPosition = finishPosition;
        }

        public static DroppingLootModel Create(DroppingLootType type, int count, Vector2 startPosition, Vector2 finishPosition)
        {
            return new DroppingLootModel(type, count, startPosition, finishPosition);
        } 
        public static DroppingLootModel FromReceivedMessage(UiLootReceivedMessage msg, Vector2 finishPosition)
        {
            return new DroppingLootModel(msg.Type, msg.Count, msg.StartPosition, finishPosition);
        }
    }
}