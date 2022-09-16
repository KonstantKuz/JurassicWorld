using UnityEngine;

namespace Dino.Loot.Messages
{
    public struct LootCollectedMessage
    {
        public string Id;
        public ReceivedItem ReceivedItem;
        public Vector3 Position;
    }
}