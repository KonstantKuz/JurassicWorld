using System.Collections.Generic;
using System.Linq;

namespace Dino.Loot.Respawn
{
    public class RespawnLootInfo
    {
        public int RespawnAmount { get; set; }
        public Queue<RespawnLoot> LootQueue { get; } = new Queue<RespawnLoot>();
        public int CurrentReceivedItemCount => LootQueue.Sum(it => it.ReceivedItem.Amount);
    }
}