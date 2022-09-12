using System;
using Dino.Inventory.Model;
using Dino.Location.Model;
using UnityEngine;

namespace Dino.Loot
{
    public class Loot : WorldObject
    {
        [SerializeField] private string _receivedItemId;    
        [SerializeField] private InventoryItemType _receivedItemType;
        [SerializeField] private int _receivedItemAmount = 1;
        
        public string ReceivedItemId => _receivedItemId;
        public InventoryItemType ReceivedItemType => _receivedItemType;
        public int ReceivedItemAmount => _receivedItemAmount;
        
        public float CollectProgress { get; private set; } = 0f;
        
        public Action<Loot> OnCollected;
        
        public void InitFromItem(Item item)
        {
            _receivedItemId = item.Id.FullName;
            _receivedItemType = item.Type;
            _receivedItemAmount = item.Amount;
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