using System;
using Dino.Location.Model;
using UnityEngine;

namespace Dino.Loot
{
    public class Loot : WorldObject
    {
        [SerializeField] private bool _autoRespawn;
        [SerializeField] private ReceivedItem _receivedItem;

        public bool AutoRespawn => _autoRespawn;
        public ReceivedItem ReceivedItem => _receivedItem;
        public float CollectProgress { get; private set; } = 0f;
        
        public Action<Loot> OnCollected;
        
        public void Init(ReceivedItem item)
        {
            _receivedItem = item;
        }

        public void IncreaseCollectProgress()
        {
            CollectProgress += Time.deltaTime;
        }

        public void ResetProgress()
        {
            CollectProgress = 0;
        }
    }
}