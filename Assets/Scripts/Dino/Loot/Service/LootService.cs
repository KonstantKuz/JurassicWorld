using System.Linq;
using Dino.Extension;
using Dino.Inventory.Model;
using Dino.Inventory.Service;
using Dino.Location;
using Dino.Location.Service;
using Dino.Loot.Messages;
using Dino.Player.Progress.Service;
using Logger.Extension;
using SuperMaxim.Messaging;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

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
        private WorldObjectFactory _worldObjectFactory;

        [Inject]
        private PlayerProgressService _playerProgressService;
        [Inject]
        private Analytics.Analytics _analytics;
        [Inject]
        private IMessenger _messenger;

        public void Collect(Loot loot)
        {
            var itemId = _inventoryService.Add(loot.ReceivedItemId, loot.ReceivedItemType, loot.ReceivedItemAmount);
            
            _playerProgressService.Progress.IncreaseLootCount();
            _analytics.ReportLootItem(itemId.FullName);
            _messenger.Publish(new LootCollectedMessage());
            
            loot.OnCollected?.Invoke(loot);
            GameObject.Destroy(loot.gameObject);
        }

        public bool CanCollect(Loot loot)
        {
            return loot.ReceivedItemType != InventoryItemType.Weapon
                   || _inventoryService.GetUniqueItemsCount(InventoryItemType.Weapon) < InventoryService.MAX_UNIQUE_WEAPONS_COUNT;
        }
        public void DropLoot(ItemId itemId)
        {
            var lootPrefab = _worldObjectFactory.GetPrefabComponents<Loot>().FirstOrDefault(it => it.ReceivedItemId == itemId.Name);
            if (lootPrefab == null) {
                this.Logger().Error($"Loot prefab not found for itemId:= {itemId}");
                return;
            }

            var lootObject = _worldObjectFactory.CreateObject(lootPrefab.gameObject).GetComponent<Loot>();
            lootObject.InitFromItem(itemId);
            var playerPosition = _world.RequirePlayer().SelfTarget.Root.position.XZ();
            var radiusFromPlayer = _world.RequirePlayer().LootCollector.CollectRadius * 2;
            SetLootPositionAndRotation(lootObject.gameObject, playerPosition, radiusFromPlayer);
        }
        private void SetLootPositionAndRotation(GameObject lootObject, Vector3 playerPosition, float radiusFromPlayer)
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