using Dino.Extension;
using Dino.Location;
using Dino.Units.Component.Animation;
using Dino.Units.Target;
using Dino.Units.Weapon;
using Feofun.Components;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Dino.Units.StateMachine
{
    public partial class UnitStateMachine : MonoBehaviour, IInitializable<IUnit>, IUpdatableComponent
    {
        //can be move to unit config, if game-designer would like to setup it
        [SerializeField] private float _rotationSpeed;
        [SerializeField] private string _currentStateName;
        private BaseState _currentState;
        
        private Unit _owner;
        private NavMeshAgent _agent;
        private Animator _animator;
        private MoveAnimationWrapper _animationWrapper;
        
        [CanBeNull] private WeaponAnimationHandler _weaponAnimationHandler;
        [CanBeNull] private ITarget _target;

        [Inject] private World _world;
        
        public ITarget Target
        {
            get => _target;
            private set
            {
                if (_target == value) return;
                if (_target != null)
                {
                    _target.OnTargetInvalid -= ClearTarget;
                }
                _target = value;
                if (_target != null)
                {
                    _target.OnTargetInvalid += ClearTarget;
                }
            }
        }

        public bool IsTargetInvalid => !Target.IsTargetValidAndAlive();

        public void Init(IUnit unit)
        {
            CacheComponents(unit);
            
            _agent.speed = _owner.Model.MoveSpeed;
            Target = _world.Player.SelfTarget;
            
            unit.OnDeath += OnDeath;
            SetState(new IdleState(this));
        }

        private void CacheComponents(IUnit unit)
        {
            _owner = (Unit) unit;
            _agent = _owner.gameObject.RequireComponent<NavMeshAgent>();
            _animator = _owner.gameObject.RequireComponentInChildren<Animator>();
            _animationWrapper = new MoveAnimationWrapper(_animator);
            _weaponAnimationHandler = _owner.gameObject.GetComponentInChildren<WeaponAnimationHandler>();
        }

        public void OnTick()
        {
            _currentState?.OnTick();
        }

        private void SetState(BaseState newState)
        {
            _currentState?.OnExitState();
            _currentState = newState;
            _currentStateName = _currentState.GetType().Name;            
            _currentState.OnEnterState();
        }

        private void OnDeath(IUnit unit, DeathCause deathCause)
        {
            unit.OnDeath -= OnDeath;
            SetState(new DeathState(this));
        }

        private void ClearTarget()
        {
            Target = null;
        }
    }
}