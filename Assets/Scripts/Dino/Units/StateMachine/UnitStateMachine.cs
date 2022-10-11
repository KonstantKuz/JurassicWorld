using System;
using Dino.Extension;
using Dino.Location.Service;
using Dino.Units.Component;
using Dino.Units.Component.Animation;
using Dino.Units.Component.Health;
using Dino.Units.Component.Target;
using Dino.Weapon;
using Feofun.Components;
using Feofun.Extension;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;


namespace Dino.Units.StateMachine
{
    public partial class UnitStateMachine : MonoBehaviour, IInitializable<Unit>, IUpdatableComponent
    {
        //can be move to unit config, if game-designer would like to setup it
        [SerializeField] private UnitState _initialState;
        [SerializeField] private string _currentStateName;
        
        private BaseState _currentState;

        private Unit _owner;
        private ITargetProvider _targetProvider;
        private IMovementController _movementController;
        private LayerMaskProvider _layerMaskProvider;
        private Animator _animator;
        private MoveAnimationWrapper _animationWrapper;
        private Health _health;
        [CanBeNull] private WeaponAnimationHandler _weaponAnimationHandler;

        [Inject] private WorldObjectFactory _worldObjectFactory;
        
        public virtual void Init(Unit unit)
        {
            CacheComponents(unit);
            SetInitialState();
        }

        private void CacheComponents(Unit unit)
        {
            _owner = unit;
            _targetProvider = _owner.gameObject.RequireComponent<ITargetProvider>();
            _movementController = _owner.gameObject.RequireComponent<IMovementController>();
            _layerMaskProvider = _owner.gameObject.RequireComponent<LayerMaskProvider>();
            _animator = _owner.gameObject.RequireComponentInChildren<Animator>();
            _animationWrapper = new MoveAnimationWrapper(_animator);
            _weaponAnimationHandler = _owner.gameObject.GetComponentInChildren<WeaponAnimationHandler>();
            _health = _owner.gameObject.RequireComponent<Health>();

            _owner.OnDeath += OnDeath;
        }

        private void SetInitialState()
        {
            SetState(_initialState);
        }

        public void OnTick()
        {
            _currentState?.OnTick();
        }

        private void SetState(UnitState state, Vector3? desiredPosition = null)
        {
            SetState(BuildState(state, desiredPosition));
        }
        
        private void SetState(BaseState newState)
        {
            _currentState?.OnExitState();
            _currentState = newState;
            _currentStateName = _currentState.GetType().Name;
            UpdateAwareness(); 
            _currentState.OnEnterState();
        }

        private void UpdateAwareness()
        {
            _health.IsUnAware = _currentState is IdleState || _currentState is PatrolState;
        }

        private void OnDeath(Unit unit, DeathCause deathCause)
        {
            unit.OnDeath -= OnDeath;
            SetState(UnitState.Death);
        }

        private BaseState BuildState(UnitState state, Vector3? desiredPosition = null)
        {
            return state switch
            {
                UnitState.Idle => new IdleState(this),
                UnitState.Patrol => new PatrolState(this),
                UnitState.Chase => new ChaseState(this),
                UnitState.Attack => new AttackState(this),
                UnitState.Death => new DeathState(this),
                UnitState.LookAround => new LookAroundState(this, desiredPosition),
                UnitState.GoToPoint => new GoToPointState(this, desiredPosition.Value),
                _ => throw new ArgumentOutOfRangeException(nameof(state), state, null)
            };
        }
        
        private void LookTowardsDamage(HitParams hitParams)
        {
            SetState(UnitState.LookAround, hitParams.AttackerPosition);
        }

        private void Stop()
        {
            if (_movementController.IsStopped) return;
            
            _movementController.IsStopped = true;
            _animationWrapper.PlayIdleSmooth();
        }

        private bool ChaseTargetIfExists()
        {
            if (_targetProvider.Target != null)
            {
                SetState(UnitState.Chase);
                return true;
            }

            return false;
        }

        public void SwitchToIdle() => SetState(UnitState.Idle);

        private void GoToPoint(Vector3 point)
        {
            _movementController.MoveTo(point);
            _animationWrapper.PlayMoveForwardSmooth();
        }
    }
}