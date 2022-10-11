using System;
using System.Linq;
using Dino.Location.Service;
using Dino.Loot.Respawn;
using UnityEngine;
using Zenject;

namespace Dino.Loot.Service
{
    public class LootFactory
    {
        [Inject]
        private WorldObjectFactory _worldObjectFactory;
        
        public Loot CreateLoot(RespawnLoot respawnLoot)
        {
            var lootPrefab = GetLootPrefab(respawnLoot.LootId).gameObject;
            var createdLoot = CreateLoot(lootPrefab, respawnLoot.ReceivedItem);
            createdLoot.transform.position = respawnLoot.Position;
            return createdLoot;
        }

        public Loot CreateLootByReceivedItem(string receivedItemName, ReceivedItem receivedItem)
        {
            var lootPrefab = GetLootPrefabByReceivedItemName(receivedItemName).gameObject;
            return CreateLoot(lootPrefab, receivedItem);
        }

        private Loot CreateLoot(GameObject prefab, ReceivedItem receivedItem)
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

        private Loot GetLootPrefabByReceivedItemName(string receivedItemName)
        {
            var lootPrefab = _worldObjectFactory.GetPrefabComponents<Loot>().FirstOrDefault(it => it.ReceivedItem.Id == receivedItemName);
            if (lootPrefab == null) {
                throw new NullReferenceException($"Loot prefab not found with received item name:= {receivedItemName}");
            }
            return lootPrefab;
        }
    }
}