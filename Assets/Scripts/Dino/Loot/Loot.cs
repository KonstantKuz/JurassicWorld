using Dino.Location.Model;
using UnityEngine;

namespace Dino.Loot
{
    public class Loot : WorldObject
    {
        [SerializeField] private string _receivedItemId;
        public float CollectProgress { get; private set; } = 0f;
        public string ReceivedItemId => _receivedItemId;

        public void IncreaseCollectProgress()
        {
            CollectProgress += Time.deltaTime;
        }
    }
}