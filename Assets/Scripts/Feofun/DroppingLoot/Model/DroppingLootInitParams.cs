using Feofun.DroppingLoot.Message;
using UnityEngine;

namespace Feofun.DroppingLoot.Model
{
    public class DroppingLootInitParams
    {
        public DroppingLootType Type { get; }
        public string LootId { get; }
        public int Count { get; }
        public Vector2 StartPosition { get; }
        public Vector2 FinishPosition { get; }

        public DroppingLootInitParams(DroppingLootType type, string lootId, int count, Vector2 startPosition, Vector2 finishPosition)
        {
            Type = type;
            Count = count;
            LootId = lootId;
            StartPosition = startPosition;
            FinishPosition = finishPosition;
        }
        public static DroppingLootInitParams FromReceivedMessage(UILootReceivedMessage msg, Vector2 finishPosition)
        {
            return new DroppingLootInitParams(msg.Type, msg.LootId, msg.Count, msg.StartPosition, finishPosition);
        }
    }
}