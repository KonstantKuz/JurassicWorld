using System.Collections.Generic;
using System.Linq;
using Dino.Location;
using Dino.Loot.Service;
using Dino.Session.Service;
using UniRx;
using UnityEngine;
using Zenject;
using Unit = Dino.Units.Unit;

namespace Dino.Loot
{
    public class LootCollector : MonoBehaviour
    {
        private const float LOOT_DESTROY_DISTANCE = 1f;
        [SerializeField] 
        private float _collectRadius = 5f;
        [SerializeField]
        private float _collectSpeed = 1;
        [SerializeField]
        private SphereCollider _collider;

        [Inject]
        private DroppingLootService _lootService;
        [Inject]
        private SessionService _sessionService;
        [Inject] 
        private World _world;

        private Unit _unit;
        private CompositeDisposable _disposable;
        private List<DroppingLoot> _movingLoots = new List<DroppingLoot>();

        private void Awake()
        {
            _collider.radius = _collectRadius;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_sessionService.SessionCompleted) {
                return;
            }
            if (!other.TryGetComponent(out DroppingLoot loot)) {
                return;
            }
            _movingLoots.Add(loot);
        }

        private void Update()
        {
            var loots = _movingLoots.ToList();
            loots.ForEach(Move);
            loots.ForEach(TryCollect);
        }

        private void Move(DroppingLoot loot)
        {
            var moveDirection = (transform.position - loot.transform.position).normalized;
            var speed = _collectSpeed;
            loot.transform.position += moveDirection * speed * Time.deltaTime;
        }

        private void TryCollect(DroppingLoot loot)
        {
            if (_world.IsPaused) return;            
            if (Vector3.Distance(loot.transform.position, transform.position) > LOOT_DESTROY_DISTANCE)
            {
                return;
            }
            
            _lootService.OnLootCollected(loot.Config);
            _movingLoots.Remove(loot);
            Destroy(loot.gameObject);
        }

        public void OnDestroy()
        {
            _movingLoots.Clear();
            _disposable?.Dispose();
            _disposable = null;
        }
    }
}