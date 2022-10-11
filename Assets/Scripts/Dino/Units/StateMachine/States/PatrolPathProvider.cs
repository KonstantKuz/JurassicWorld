using Dino.Location;
using JetBrains.Annotations;
using Logger.Extension;
using UnityEngine;
using Zenject;

namespace Dino.Units.StateMachine.States
{
    public class PatrolPathProvider : MonoBehaviour
    {
        private PatrolPath _patrolPath;
        
        [Inject] private World _world;

        [CanBeNull] public PatrolPath PatrolPath => _patrolPath;

        public void InitPatrolPath()
        {
            if (_patrolPath != null) return;
            
            _patrolPath = FindNearestFreePath();
            if (_patrolPath == null)
            {
                this.Logger().Warn($"Patrol path was not found for {gameObject.name}");
                return;
            }
            _patrolPath.IsBusy = true;
        }

        [CanBeNull]
        private PatrolPath FindNearestFreePath()
        {
            PatrolPath freePath = null;
            var patrolPaths = _world.GetChildrenComponents<PatrolPath>();
            var minDistance = Mathf.Infinity;
            foreach (var path in patrolPaths)
            {
                if (path.IsBusy || path.Path.Length == 0)
                {
                    continue;
                }
                
                var distance = Vector3.Distance(transform.position, path.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    freePath = path;
                }
            }

            return freePath;
        }
        
    }
}