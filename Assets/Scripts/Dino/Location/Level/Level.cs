using System;
using System.Collections.Generic;
using System.Linq;
using Dino.Location.Model;
using Dino.Units;
using UnityEngine;

namespace Dino.Location.Level
{
    public class Level : WorldObject
    {
        private const string GROUND_ROOT_NAME = "Ground";
        
        [SerializeField] private Transform _start;
        [SerializeField] private Trigger _finish;
        [SerializeField] private Transform _groundRoot;
        
        private Bounds[] _groundBounds;
        private List<Unit> _enemies;
        public Transform Start => _start;
        public List<Unit> Enemies =>
            _enemies ??= GetComponentsInChildren<Unit>().Where(it => it.UnitType == UnitType.ENEMY).ToList();
        public Bounds[] GroundBounds =>
            _groundBounds ??= _groundRoot.GetComponentsInChildren<Renderer>().Select(it => it.bounds).ToArray();

        public event Action OnPlayerTriggeredFinish;

        private void OnValidate()
        {
            _groundRoot = transform.Find(GROUND_ROOT_NAME);
        }

        private void Awake()
        {
            _finish.OnTriggerEnterCallback += OnFinishTriggered;
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
        }
    }
}
