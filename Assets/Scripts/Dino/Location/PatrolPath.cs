using System;
using System.Linq;
using ModestTree;
using UnityEngine;

namespace Dino.Location
{
    public class PatrolPath : MonoBehaviour
    {
        private Transform[] _path;
        public Transform[] Path => _path ??= GetComponentsInChildren<Transform>().Except(transform).ToArray();
        public bool IsBusy { get; set; }

        public bool IsEndOfPath(Transform point)
        {
            return _path.Length > 0 && (_path[0] == point || _path[_path.Length - 1] == point);
        }
    }
}
