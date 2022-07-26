using System;
using Dino.Extension;
using Dino.Location;
using Dino.Units.Component;
using Dino.Units.Component.Animation;
using Dino.Units.Component.Health;
using Dino.Units.Component.Target;
using Dino.Weapon;
using Feofun.Components;
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
        private Animator _animator;
        private MoveAnimationWrapper _animationWrapper;
        [CanBeNull] private WeaponAnimationHandler _weaponAnimationHandler;
        private float _waitTimer;
        
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
            return state switch
            {
                UnitState.Idle => new IdleState(this),
                UnitState.Patrol => new PatrolState(this),
                UnitState.Chase => new ChaseState(this),
                UnitState.Attack => new AttackState(this),
                UnitState.Death => new DeathState(this),
                UnitState.LookAround => new LookAroundState(this),
                _ => throw new ArgumentOutOfRangeException(nameof(state), state, null)
            };
        }
        
        private void LookAround(DamageParams damageParams)
        {
            SetState(new LookAroundState(this, damageParams.Position));
        }

        private void Wait(float waitTime, Action onWaitTimeEnd)
        {
            _waitTimer += Time.deltaTime;

            if (_waitTimer >= waitTime)
            {
                onWaitTimeEnd?.Invoke();
                return;
            }
            
            if (_movementController.IsStopped) return;
            
            _movementController.IsStopped = true;
            _animationWrapper.PlayIdleSmooth();
        }

        private void ChaseTargetIfExists()
        {
            if (_targetProvider.Target != null)
            {
                SetState(UnitState.Chase);
            }
        }
    }
}