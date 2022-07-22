using System;
using System.Collections.Generic;
using System.Linq;
using Dino.Location.Model;
using Dino.Units;
using UnityEngine;

namespace Dino.Location
{
    public class Level : WorldObject
    {
        [SerializeField] private Transform _start;
        [SerializeField] private Trigger _finish;

        private List<Unit> _enemies;
        public Transform Start => _start;
        public List<Unit> Enemies =>
            _enemies ??= GetComponentsInChildren<Unit>().Where(it => it.UnitType == UnitType.ENEMY).ToList();
        
        public event Action OnPlayerTriggeredFinish;

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
