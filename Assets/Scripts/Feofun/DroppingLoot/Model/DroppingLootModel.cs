using Feofun.DroppingLoot.Message;
using UnityEngine;

namespace Feofun.DroppingLoot.Model
{
    public class DroppingLootModel
    {
        public DroppingLootType Type { get; }

        public string LootId { get; }
        public int Count { get; }
        public Vector2 StartPosition { get; }
        public Vector2 FinishPosition { get; }

        public DroppingLootModel(DroppingLootType type, string lootId, int count, Vector2 startPosition, Vector2 finishPosition)
        {
            Type = type;
            Count = count;
            LootId = lootId;
            StartPosition = startPosition;
            FinishPosition = finishPosition;
        }

        public static DroppingLootModel Create(DroppingLootType type, string lootId, int count, Vector2 startPosition, Vector2 finishPosition)
        {
            return new DroppingLootModel(type, lootId, count, startPosition, finishPosition);
        } 
        public static DroppingLootModel FromReceivedMessage(UiLootReceivedMessage msg, Vector2 finishPosition)
        {
            return new DroppingLootModel(msg.Type, msg.LootId, msg.Count, msg.StartPosition, finishPosition);
        }
    }
}