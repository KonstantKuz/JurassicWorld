using System.Linq;
using Dino.Extension;
using Dino.Inventory.Model;
using Dino.Inventory.Service;
using Dino.Location;
using Dino.Location.Service;
using Dino.Units.Service;
using Logger.Extension;
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
        private ActiveItemService _activeItemService;

        [Inject]
        private World _world;
        [Inject]
        private WorldObjectFactory _worldObjectFactory;


        public void Collect(Loot loot)
        {
            var itemId = _inventoryService.Add(loot.ReceivedItemId);
            _activeItemService.Replace(itemId);
        }

        public void DropLoot(ItemId itemId)
        {
            var lootPrefab = _worldObjectFactory.GetPrefabComponents<Loot>().FirstOrDefault(it => it.ReceivedItemId == itemId.Name);
            if (lootPrefab == null) {
                this.Logger().Error($"Loot prefab not found for itemId:= {itemId}");
                return;
            }
            var lootCollector = _world.Player.gameObject.RequireComponentInChildren<LootCollector>();
            var lootObject = _worldObjectFactory.CreateObject(lootPrefab.gameObject);
            var playerPosition = _world.Player.SelfTarget.Root.position.XZ();
            var radiusFromPlayer = lootCollector.CollectRadius * 2;
            SetLootPosition(playerPosition, lootObject, radiusFromPlayer);
        }

        private void SetLootPosition(Vector3 playerPosition, GameObject lootObject, float radius)
        {
            lootObject.transform.SetPositionAndRotation(IsPlaceEmpty(playerPosition, radius, out var result) ? result : playerPosition,
                                                        Quaternion.identity);
        }

        private bool IsPlaceEmpty(Vector3 center, float range, out Vector3 result)
        {
            for (int angle = 0; angle <= SEARCH_POSITION_ANGLE_MAX; angle += SEARCH_POSITION_ANGLE_STEP) {
                var point = center + GetPointOnCircle(angle) * range;
                if (!NavMesh.SamplePosition(point, out var hit, 1f, NavMesh.AllAreas)) {
                    continue;
                }
                result = hit.position;
                return true;
            }
            result = Vector3.zero;
            return false;
        }
        private Vector3 GetPointOnCircle(float angle)
        {
            float radAngle = Mathf.Deg2Rad * angle;
            return new Vector3(Mathf.Cos(radAngle), 0, Mathf.Sin(radAngle));
        }
    }
}