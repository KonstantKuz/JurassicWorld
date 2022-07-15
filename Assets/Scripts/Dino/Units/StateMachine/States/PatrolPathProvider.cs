using System;
using Dino.Location;
using JetBrains.Annotations;
using Logger.Extension;
using UnityEngine;
using Zenject;

namespace Dino.Units.StateMachine.States
{
    public enum PatrolBehaviourType
    {
        Loop,
        PingPong,
    }

    public class PatrolPathProvider : MonoBehaviour
    {
        [SerializeField] public PatrolBehaviourType _patrolBehaviourType;

        private PatrolPath _patrolPath;
        private int _nextPointIndex = -1;
        private bool _reverse;
        
        [Inject] private World _world;

        [CanBeNull] public PatrolPath PatrolPath => _patrolPath;

        public void Awake()
        {
            InitPatrolPath();
        }

        private void InitPatrolPath()
        {
            var patrolPaths = _world.GetChildrenComponents<PatrolPath>();
            var minDistance = Mathf.Infinity;
            foreach (var path in patrolPaths)
            {
                if (path.IsBusy || path.Path.Length == 0)
                {
                    continue;
                }
                
                var distance = Vector3.Distance(transform.position, path.Path[0].position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    _patrolPath = path;
                }
            }

            if (_patrolPath == null)
            {
                this.Logger().Warn($"Patrol path was not found for {gameObject.name}");
                return;
            }
            
            _patrolPath.IsBusy = true;
        }
        
        public Transform Pop()
        {
            switch (_patrolBehaviourType)
            {
                case PatrolBehaviourType.Loop:
                    return GetNextLoopPoint();
                case PatrolBehaviourType.PingPong:
                    return GetNextPingPongPoint();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private Transform GetNextLoopPoint()
        {
            _nextPointIndex++;
            if (_nextPointIndex >= PatrolPath.Path.Length)
            {
                _nextPointIndex = 0;
            }
            return PatrolPath.Path[_nextPointIndex];
        }

        private Transform GetNextPingPongPoint()
        {
            _nextPointIndex += _reverse ? -1 : 1;
            if (_nextPointIndex == 0)
            {
                _reverse = false;
            }
            if (_nextPointIndex == PatrolPath.Path.Length - 1)
            {
                _reverse = true;
            }
            return PatrolPath.Path[_nextPointIndex];
        }
    }
}