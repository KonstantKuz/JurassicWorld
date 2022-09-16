using Feofun.DroppingLoot.Model;
using UnityEngine;

namespace Feofun.DroppingLoot.Message
{
    public struct UiLootReceivedMessage
    {
        public int Count { get; }
        public DroppingLootType Type { get; }
        
        public string LootId { get; }
        public Vector2 StartPosition { get; }
        
        public UiLootReceivedMessage(DroppingLootType type, string lootId, int count, Vector2 startPosition)
        {
            Type = type;
            Count = count;
            StartPosition = startPosition;
            LootId = lootId;
        }
        public static UiLootReceivedMessage Create(DroppingLootType type, string lootId, int count, Vector2 startPosition)
        {
            return new UiLootReceivedMessage(type, lootId, count, startPosition);
        }
    }
}