using Dino.Inventory.Model;
using Dino.Inventory.Service;
using Dino.Location;
using Dino.Loot.Service;
using Dino.Session.Service;
using UnityEngine;
using Zenject;

namespace Dino.Loot
{
    [RequireComponent(typeof(LootHudOwner))]
    public class LootCollector : MonoBehaviour
    {
        [SerializeField] private float _collectRadius = 1.5f;
        [SerializeField] private float _collectTime = 1f;
        [SerializeField] private SphereCollider _collider;
        
        private LootHudOwner _lootHud;

        [Inject] private LootService _lootService;
        [Inject] private SessionService _sessionService;
        [Inject] private World _world;
        [Inject] private InventoryService _inventoryService;

        public float CollectRadius => _collectRadius;
        
        private void Awake()
        {
            _collider.radius = _collectRadius;
            _lootHud = GetComponent<LootHudOwner>();
        }

        private void OnTriggerStay(Collider other)
        {
            if (_sessionService.SessionCompleted) {
                return;
            }
            if (!other.TryGetComponent(out Loot loot)) {
                return;
            }
            TryCollect(loot);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent(out Loot loot)) {
                return;
            }

            loot.ResetProgress();
            _lootHud.Hide();
        }

        private void TryCollect(Loot loot)
        {
            if (_world.IsPaused || _inventoryService.GetUniqueItemsCount(InventoryItemType.Weapon) >= InventoryService.MAX_UNIQUE_WEAPONS_COUNT) {
                return;
            }
            
            loot.IncreaseCollectProgress();
            _lootHud.ShowProgress(loot.CollectProgress / _collectTime);
            
            if (loot.CollectProgress <= _collectTime) return;
      
            _lootHud.Hide();
            _lootService.Collect(loot);
        }
    }
}
