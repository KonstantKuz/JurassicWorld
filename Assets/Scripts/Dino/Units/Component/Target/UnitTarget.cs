using System;
using UnityEngine;
using Zenject;

namespace Dino.Units.Component.Target
{
    public class UnitTarget : MonoBehaviour, ITarget, IUnitDeathEventReceiver
    {
        private enum State
        {
            Default,
            Hidden,
            Dead
        }
        
        private static int _idCount;
        
        [SerializeField]
        private UnitType _unitType;
        [SerializeField] 
        private Transform _centerTarget;   
        [SerializeField] 
        private Transform _rootTarget;

        [Inject] 
        private TargetService _targetService;

        private State _state;

        public string TargetId { get; private set; }
        public bool IsValid => _state == State.Default;
        public Transform Root => _rootTarget;
        public Transform Center => _centerTarget;
        public UnitType UnitType
        {
            get => _unitType;
            set
            {
                _targetService.Remove(this);
                _unitType = value;
                _targetService.Add(this);
            }
        }
        public Action OnTargetInvalid { get; set; }

        public bool Hidden
        {
            get => _state == State.Hidden;
            set
            {
                if (_state == State.Dead) return;
                _state = value ? State.Hidden : State.Default;
            }
        }

        private void Awake()
        {
            TargetId = $"{_unitType.ToString()}#{_idCount++}";
            _targetService.Add(this);
        }

        public void OnDeath(DeathCause deathCause)
        {
            if (_state == State.Dead) return;
            _state = State.Dead;
            _targetService.Remove(this);            
            OnTargetInvalid?.Invoke();
        }

        private void OnDestroy()
        {
            if (_state == State.Dead) return;
            _targetService.Remove(this);
            OnTargetInvalid?.Invoke();
        }
    }
}