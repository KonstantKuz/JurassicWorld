using System;
using System.Collections.Generic;
using System.Linq;
using Dino.Location.Model;
using Dino.Location.Service;
using Dino.Loot.Messages;
using Dino.Session.Messages;
using Dino.Units;
using Feofun.Extension;
using SuperMaxim.Messaging;
using UniRx;
using UnityEngine;
using Zenject;
using Unit = Dino.Units.Unit;

namespace Dino.Location.Level
{
    public class Level : WorldObject
    {
        private const string GROUND_ROOT_NAME = "Ground";
        
        [SerializeField] private Transform _start;
        [SerializeField] private Trigger _finish;

        private CompositeDisposable _disposable;
        private Transform _groundRoot;
        private Bounds[] _groundBounds;
        private List<Unit> _enemies;
        private ArrowIndicator _lootIndicator;
        
        [Inject] private IMessenger _messenger;
        [Inject] private WorldObjectFactory _worldObjectFactory;
        
        private Transform GroundRoot => _groundRoot ??= transform.Find(GROUND_ROOT_NAME);
        private Bounds[] GroundBounds =>
            _groundBounds ??= GroundRoot.GetComponentsInChildren<Renderer>()
                .Where(it => it.gameObject.layer == LayerMask.NameToLayer("Ground"))
                .Select(it => it.bounds).ToArray();

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
        
        public void Init()
        {
            Dispose();
            _disposable = new CompositeDisposable();
            _finish.OnTriggerEnterCallback += OnFinishTriggered;
            _messenger.SubscribeWithDisposable<AllEnemiesKilledMessage>(SpawnIndicatorAboveFinish).AddTo(_disposable);
        }

        private void SpawnIndicatorAboveFinish(AllEnemiesKilledMessage _)
        {
            ArrowIndicator.SpawnAbove(_worldObjectFactory, _finish.transform, Vector3.zero);
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
            Dispose();
        }

        private void Dispose()
        {
            _finish.OnTriggerEnterCallback -= OnFinishTriggered;
            
            _disposable?.Dispose();
            _disposable = null;
        }
    }
}
