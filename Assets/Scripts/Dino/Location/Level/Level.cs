using System;
using System.Collections.Generic;
using System.Linq;
using Dino.Location.Model;
using Dino.Location.Service;
using Dino.Loot.Messages;
using Dino.Session.Messages;
using Dino.Units;
using SuperMaxim.Messaging;
using UnityEngine;
using Zenject;

namespace Dino.Location.Level
{
    public class Level : WorldObject
    {
        private static Vector3 ARROW_ITEM_OFFSET = Vector3.up * 2 + Vector3.forward / 2;
        private const string GROUND_ROOT_NAME = "Ground";
        
        [SerializeField] private Transform _start;
        [SerializeField] private Trigger _finish;

        private int _levelNumber;
        private Transform _groundRoot;
        private Bounds[] _groundBounds;
        private List<Unit> _enemies;
        private ArrowIndicator _lootIndicator;
        
        [Inject] private IMessenger _messenger;
        [Inject] private WorldObjectFactory _worldObjectFactory;
        
        private Transform GroundRoot => _groundRoot ??= transform.Find(GROUND_ROOT_NAME);
        private Bounds[] GroundBounds =>
            _groundBounds ??= GroundRoot.GetComponentsInChildren<Renderer>().Select(it => it.bounds).ToArray();

        public Transform Start => _start;
        public List<Unit> Enemies =>
            _enemies ??= GetComponentsInChildren<Unit>().Where(it => it.UnitType == UnitType.ENEMY).ToList();

        public event Action OnPlayerTriggeredFinish;

        public Bounds GetBounds()
        {
            var levelBounds = new Bounds();
            foreach (var bounds in GroundBounds) 
            {
                levelBounds.Encapsulate(bounds);
            }
            return levelBounds;
        }
        
        public void Init(int levelNumber)
        {
            _levelNumber = levelNumber;
            SpawnIndicatorAboveLoot();
            _finish.OnTriggerEnterCallback += OnFinishTriggered;
            _messenger.Subscribe<AllEnemiesKilledMessage>(SpawnIndicatorAboveFinish);
        }

        private void SpawnIndicatorAboveLoot()
        {
            if (_levelNumber != 0) return;
            
            var loot = GetComponentInChildren<Loot.Loot>();
            _lootIndicator = ArrowIndicator.SpawnAbove(_worldObjectFactory, loot.transform, ARROW_ITEM_OFFSET);
            _messenger.Subscribe<LootCollectedMessage>(RemoveIndicatorAboveLoot);
        }

        private void SpawnIndicatorAboveFinish(AllEnemiesKilledMessage _)
        {
            ArrowIndicator.SpawnAbove(_worldObjectFactory, _finish.transform, ARROW_ITEM_OFFSET);
        }

        private void RemoveIndicatorAboveLoot(LootCollectedMessage _)
        {
            Destroy(_lootIndicator.gameObject);
        }
        
        private void OnFinishTriggered(Collider other)
        {
            if (other.TryGetComponent(out Unit unit) && unit.UnitType == UnitType.PLAYER)
            {
                OnPlayerTriggeredFinish?.Invoke();
            }
        }

        private void OnDestroy()
        {
            _finish.OnTriggerEnterCallback -= OnFinishTriggered;
            _messenger.Unsubscribe<AllEnemiesKilledMessage>(SpawnIndicatorAboveFinish);
            if (_levelNumber == 0)
            {
                _messenger.Unsubscribe<LootCollectedMessage>(RemoveIndicatorAboveLoot);
            }
        }
    }
}
