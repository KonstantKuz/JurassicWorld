using Feofun.DroppingLoot.Model;
using UnityEngine;

namespace Feofun.DroppingLoot.Message
{
    public struct UiLootReceivedMessage
    {
        public int Count { get; }
        public DroppingLootType Type { get; }
        public Vector2 StartPosition { get; }
        
        public UiLootReceivedMessage(DroppingLootType type, int count, Vector2 startPosition)
        {
            Type = type;
            Count = count;
            StartPosition = startPosition;
        }
        public static UiLootReceivedMessage Create(DroppingLootType type, int count, Vector2 startPosition)
        {
            return new UiLootReceivedMessage(type, count, startPosition);
        } 
        public static UiLootReceivedMessage FromReward(RewardItem reward, Vector2 startPosition)
        {
            return new UiLootReceivedMessage(DroppingLootTypeExt.ValueOf(reward.RewardId), reward.Count, startPosition);
        }
    }
}