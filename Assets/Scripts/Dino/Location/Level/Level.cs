using System;
using System.Collections.Generic;
using System.Linq;
using Dino.Location.Model;
using Dino.Location.Service;
using Dino.Session.Messages;
using Dino.Session.Service;
using Dino.Units;
using SuperMaxim.Messaging;
using UnityEngine;
using Zenject;

namespace Dino.Location.Level
{
    public class Level : WorldObject
    {
        private const string GROUND_ROOT_NAME = "Ground";
        
        [SerializeField] private Transform _start;
        [SerializeField] private Trigger _finish;
        
        private Transform _groundRoot;
        private Bounds[] _groundBounds;
        private List<Unit> _enemies;

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
        
        private void Awake()
        {
            _finish.OnTriggerEnterCallback += OnFinishTriggered;
            _messenger.Subscribe<AllEnemiesKilledMessage>(SpawnIndicator);
        }

        private void SpawnIndicator(AllEnemiesKilledMessage msg)
        {
            var indicatorPrefab = _worldObjectFactory.GetPrefabComponents<ArrowIndicator>().First();
            var indicator = _worldObjectFactory.CreateObject(indicatorPrefab.gameObject).GetComponent<ArrowIndicator>();
            indicator.PointAt(_finish.transform);
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
            _messenger.Unsubscribe<AllEnemiesKilledMessage>(SpawnIndicator);
        }
    }
}
