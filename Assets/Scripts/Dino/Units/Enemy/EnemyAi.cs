using Dino.Extension;
using Dino.Location;
using Dino.Units.Component.TargetSearcher;
using Dino.Units.Enemy.Model;
using Dino.Units.Target;
using Feofun.Components;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Dino.Units.Enemy
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyAi : MonoBehaviour, IInitializable<IUnit>, IUpdatableComponent, IUnitDeactivateEventReceiver
    {
        private NavMeshAgent _agent;

        [Inject] private World _world;

        public ITarget Target => _world.Player.SelfTarget;
        private Vector3 TargetPosition => Target.Root.position;
        public float DistanceToTarget => Vector3.Distance(transform.position, TargetPosition);
        
        public void Init(IUnit unit)
        {
            _agent.speed = unit.Model.MoveSpeed;
        }
        private void Awake()
        {
            _agent = gameObject.RequireComponent<NavMeshAgent>();
        }

        public void OnTick()
        {
            if (_world.Player == null) return;
            MoveTo(TargetPosition);
        }

        private void MoveTo(Vector3 destination)
        {
            _agent.destination = destination;
        }

        public void OnDeactivate()
        {
            _agent.isStopped = true;
        }
    }
}