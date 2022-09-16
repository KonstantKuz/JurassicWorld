using Feofun.DroppingLoot.Model;
using UnityEngine;

namespace Feofun.DroppingLoot.Message
{
    public struct UILootReceivedMessage
    {
        public DroppingLootType Type { get; }
        public string LootId { get; }
        public int Count { get; }
        public Vector2 StartPosition { get; }

        public UILootReceivedMessage(DroppingLootType type, string lootId, int count, Vector2 startPosition)
        {
            Type = type;
            Count = count;
            StartPosition = startPosition;
            LootId = lootId;
        }

        public static UILootReceivedMessage Create(DroppingLootType type, string lootId, int count, Vector2 startPosition)
        {
            return new UILootReceivedMessage(type, lootId, count, startPosition);
        }
    }
}