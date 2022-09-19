using System;
using System.Linq;
using Dino.Extension;
using Dino.Inventory.Model;
using Dino.Inventory.Service;
using Dino.Location;
using Dino.Location.Service;
using Dino.Loot.Messages;
using Dino.Player.Progress.Service;
using GluonGui.WorkspaceWindow.Views.WorkspaceExplorer.Search;
using JetBrains.Annotations;
using Logger.Extension;
using SuperMaxim.Messaging;
using UnityEngine;
using UnityEngine.AI;
using Zenject;
using Object = UnityEngine.Object;

namespace Dino.Loot.Service
{
    public class LootService
    {
        private const int SEARCH_POSITION_ANGLE_STEP = 10;
        private const int SEARCH_POSITION_ANGLE_MAX = 360;

        [Inject]
        private InventoryService _inventoryService;
        [Inject] 
        private World _world;
        [Inject]
        private LootFactory _lootFactory;
        [Inject]
        private LootRespawnService _lootRespawnService;
        [Inject]
        private PlayerProgressService _playerProgressService;
        [Inject]
        private Analytics.Analytics _analytics;
        [Inject]
        private IMessenger _messenger;

        public void Collect(Loot loot)
        {
            var itemId = _inventoryService.Add(ItemId.Create(loot.ReceivedItem.Id), loot.ReceivedItem.Type, loot.ReceivedItem.Amount);
            
            _playerProgressService.Progress.IncreaseLootCount();
            _analytics.ReportLootItem(itemId.Id.FullName);

            if (loot.AutoRespawn) {
                _lootRespawnService.AddToRespawn(loot.ObjectId, loot.ReceivedItem, loot.transform.position);
            }
            
            loot.OnCollected?.Invoke(loot);
            _messenger.Publish(new LootCollectedMessage());

            Object.Destroy(loot.gameObject);
        }

        public bool CanCollect(Loot loot)
        {
            return loot.ReceivedItem.Type != InventoryItemType.Weapon
                   || _inventoryService.GetUniqueItemsCount(InventoryItemType.Weapon) < InventoryService.MAX_UNIQUE_WEAPONS_COUNT;
        }
        public void DropLoot(Item item)
        {
            var receivedItem = ReceivedItem.CreateFromItem(item);
            var lootPrefab = _lootFactory.GetLootPrefabByReceivedItemName(item.Name);
            var loot = _lootFactory.CreateLoot(lootPrefab.gameObject, receivedItem);
            var playerPosition = _world.RequirePlayer().SelfTarget.Root.position.XZ();
            var radiusFromPlayer = _world.RequirePlayer().LootCollector.CollectRadius * 2;
            PlaceLootNearPlayer(loot.gameObject, playerPosition, radiusFromPlayer);
        }

        private void PlaceLootNearPlayer(GameObject lootObject, Vector3 playerPosition, float radiusFromPlayer)
        {
            lootObject.transform.SetPositionAndRotation(GetLootSpawnPosition(playerPosition, radiusFromPlayer).XZ(), Quaternion.identity);
        }

        private Vector3 GetLootSpawnPosition(Vector3 center, float range)
        {
            for (int angle = 0; angle <= SEARCH_POSITION_ANGLE_MAX; angle += SEARCH_POSITION_ANGLE_STEP) {
                var point = center + GetPointOnCircle(angle) * range;
                if (!NavMesh.SamplePosition(point, out var hit, 1f, NavMesh.AllAreas)) {
                    continue;
                }
                return hit.position;
            }
            return center;
        }

        private Vector3 GetPointOnCircle(float angle)
        {
            float radAngle = Mathf.Deg2Rad * angle;
            return new Vector3(Mathf.Cos(radAngle), 0, Mathf.Sin(radAngle));
        }
    }
}