using System.Linq;
using ModestTree;
using UnityEngine;
using UnityEngine.AI;

namespace Dino.Location
{
    public class PatrolPath : MonoBehaviour
    {
        private const float SNAP_DISTANCE = 2f;
        private Transform[] _path;
        public Transform[] Path => _path ??= GetComponentsInChildren<Transform>().Except(transform).ToArray();
        public bool IsBusy { get; set; }

        private void Awake()
        {
            SnapPathToNavMesh();
        }

        private void SnapPathToNavMesh()
        {
            foreach (var point in Path)
            {
                NavMesh.SamplePosition(point.position, out var hit, SNAP_DISTANCE, NavMesh.AllAreas);
                point.position = hit.position;
            }
        }

        public bool IsEndOfPath(Transform point)
        {
            return _path.Length > 0 && (_path[0] == point || _path[_path.Length - 1] == point);
        }
    }
}
