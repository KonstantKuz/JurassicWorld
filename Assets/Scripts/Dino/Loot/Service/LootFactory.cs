using System;
using System.Linq;
using Dino.Location.Service;
using UnityEngine;
using Zenject;

namespace Dino.Loot.Service
{
    public class LootFactory
    {
        [Inject]
        private WorldObjectFactory _worldObjectFactory;
        
        public Loot SpawnLoot(string lootId, ReceivedItem receivedItem)
        {
            var lootPrefab = GetLootPrefab(lootId).gameObject;
            return SpawnLoot(lootPrefab, receivedItem);
        }

        public Loot SpawnLoot(GameObject prefab, ReceivedItem receivedItem)
        {
            var lootObject = _worldObjectFactory.CreateObject<Loot>(prefab);
            lootObject.Init(receivedItem);
            return lootObject;
        }

        private Loot GetLootPrefab(string lootId)
        {
            var lootPrefab = _worldObjectFactory.GetPrefabComponents<Loot>().FirstOrDefault(it => it.ObjectId == lootId);
            if (lootPrefab == null) {
                throw new NullReferenceException($"Loot prefab not found with id:= {lootId}");
            }
            return lootPrefab;
        }

        public Loot GetLootPrefabByReceivedItemName(string receivedItemName)
        {
            var lootPrefab = _worldObjectFactory.GetPrefabComponents<Loot>().FirstOrDefault(it => it.ReceivedItem.Id == receivedItemName);
            if (lootPrefab == null) {
                throw new NullReferenceException($"Loot prefab not found with received item name:= {receivedItemName}");
            }
            return lootPrefab;
        }
    }
}