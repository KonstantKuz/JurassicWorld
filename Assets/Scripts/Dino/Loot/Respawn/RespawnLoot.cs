using UnityEngine;

namespace Dino.Loot.Respawn
{
    public class RespawnLoot
    {
        public string LootId { get; }
        public ReceivedItem ReceivedItem { get; }
        public Vector3 Position { get; }
        
        public RespawnLoot(Loot loot)
        {
            LootId = loot.ObjectId;
            ReceivedItem = loot.ReceivedItem;
            Position = loot.transform.position;
        }
    }
}