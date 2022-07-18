using System;
using Dino.Location.Model;
using Dino.Units;
using Dino.Units.Player.Attack;
using UnityEngine;

namespace Dino.Location
{
    public class Level : WorldObject
    {
        [SerializeField] private Transform _start;
        [SerializeField] private Trigger _finish;
        
        public Transform Start => _start;
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
