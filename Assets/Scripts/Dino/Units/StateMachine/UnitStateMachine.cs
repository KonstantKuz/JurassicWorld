using System;
using Dino.Extension;
using Dino.Units.Component;
using Dino.Units.Component.Animation;
using Dino.Units.Component.Target;
using Dino.Weapon;
using Feofun.Components;
using JetBrains.Annotations;
using UnityEngine;


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
        private Animator _animator;
        private MoveAnimationWrapper _animationWrapper;
        [CanBeNull] private WeaponAnimationHandler _weaponAnimationHandler;

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
            _animator = _owner.gameObject.RequireComponentInChildren<Animator>();
            _animationWrapper = new MoveAnimationWrapper(_animator);
            _weaponAnimationHandler = _owner.gameObject.GetComponentInChildren<WeaponAnimationHandler>();

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

        private void SetState(UnitState state)
        {
            SetState(BuildState(state));
        }
        
        private void SetState(BaseState newState)
        {
            _currentState?.OnExitState();
            _currentState = newState;
            _currentStateName = _currentState.GetType().Name;            
            _currentState.OnEnterState();
        }

        private void OnDeath(Unit unit, DeathCause deathCause)
        {
            unit.OnDeath -= OnDeath;
            SetState(UnitState.Death);
        }

        private BaseState BuildState(UnitState state)
        {
            switch (state)
            {
                case UnitState.Idle:
                    return new IdleState(this);
                case UnitState.Patrol:
                    return new PatrolState(this);
                case UnitState.Chase:
                    return new ChaseState(this);
                case UnitState.Attack:
                    return new AttackState(this);
                case UnitState.Death:
                    return new DeathState(this);
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }
    }
}