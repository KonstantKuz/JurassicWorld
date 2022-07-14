using Dino.Location;
using UnityEngine;
using Zenject;

namespace Dino.Units.StateMachine.States
{
    public class PatrolStateHelper : MonoBehaviour
    {
        private PatrolPath _patrolPath;

        [Inject] private World _world;

        public PatrolPath PatrolPath => _patrolPath;

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

            _patrolPath.IsBusy = true;
        }
    }

}