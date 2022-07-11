﻿using DinoWorldSurvival.Extension;
using DinoWorldSurvival.Location;
using DinoWorldSurvival.Units.Component.TargetSearcher;
using DinoWorldSurvival.Units.Enemy.Model;
using DinoWorldSurvival.Units.Target;
using Feofun.Components;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace DinoWorldSurvival.Units.Enemy
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyAi : MonoBehaviour, IInitializable<IUnit>, IUpdatableComponent, IUnitDeactivateEventReceiver
    {
        private const float ACCURATE_FOLLOW_DISTANCE = 1f;
        
        [SerializeField] private float _targetSelectionDistance = 10f;
        [SerializeField] private float _agentRadiusAfar;
        [SerializeField] private float _agentRadiusNear;
        [SerializeField] private float _agentDistanceAfar;
        [SerializeField] private float _agentDistanceNear;

        private ITarget _selfTarget;
        private CapsuleCollider _collider;
        private NavMeshAgent _agent;
        private ITarget _target;
        private ITargetSearcher _targetSearcher;
        private float _initialAgentRadius;

        [Inject] private World _world;

        private Vector3 SquadPosition => _world.Squad.Destination.transform.position;
        private float AgentRadiusAfar => _agentRadiusAfar / Scale;   
        private float AgentRadiusNear => _agentRadiusNear / Scale;
        private float Scale => transform.localScale.x;
        private float SelfRadius => _collider.radius * Scale;
        private float DistanceToSquad => Vector3.Distance(_selfTarget.Root.position, SquadPosition) - SelfRadius;
        public float DistanceToTarget => CurrentTarget == null ? float.MaxValue : Vector3.Distance(_selfTarget.Root.position, CurrentTarget.Root.position) - SelfRadius;

        public NavMeshAgent NavMeshAgent => _agent;
        
        [CanBeNull] 
        public ITarget CurrentTarget
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

        public void Init(IUnit unit)
        {
            var model = (EnemyUnitModel) unit.Model;
            _agent.speed = model.MoveSpeed;
        }
        private void Awake()
        {
            _agent = gameObject.RequireComponent<NavMeshAgent>();
            _selfTarget = gameObject.RequireComponent<ITarget>();
            _targetSearcher = gameObject.RequireComponent<ITargetSearcher>();
            _collider = gameObject.RequireComponent<CapsuleCollider>();
            _initialAgentRadius = _agent.radius;
        }

        public void OnTick()
        {
            if (_world.Squad == null) return;
            
            UpdateAgentRadius();
            
            if (DistanceToSquad > _targetSelectionDistance) 
            {
                MoveTo(SquadPosition);
                return;
            }
            
            FindTarget();

            if (CurrentTarget == null)
            {
                _agent.isStopped = true;
                return;
            }

            MoveTo(CurrentTarget.Root.position);
            _agent.isStopped = false;
        }

        private void MoveTo(Vector3 destination)
        {
            if (Vector3.Distance(transform.position, destination) > ACCURATE_FOLLOW_DISTANCE)
            {
                _agent.destination = transform.position + (destination - transform.position).normalized;
            }
            else
            {
                _agent.destination = destination;
            }
        }

        private void UpdateAgentRadius()
        {
            _agent.radius = _initialAgentRadius + Mathf.Lerp(AgentRadiusNear, AgentRadiusAfar, 
                                                             (DistanceToSquad - _agentDistanceNear) / (_agentDistanceAfar - _agentDistanceNear));
        }

        private void FindTarget()
        {
            CurrentTarget = _targetSearcher.Find();
        }

        private void ClearTarget()
        {
            CurrentTarget = null;
        }

        public void OnDeactivate()
        {
            _agent.isStopped = true;
        }
    }
}