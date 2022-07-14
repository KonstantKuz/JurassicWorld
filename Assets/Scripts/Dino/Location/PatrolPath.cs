using System;
using System.Linq;
using ModestTree;
using UnityEngine;

namespace Dino.Location
{
    public enum PathType
    {
        Loop,
        PingPong,
    }
    
    public class PatrolPath : MonoBehaviour
    {
        [SerializeField] private bool _waitOnEveryPoint;
        [SerializeField] public PathType _pathType;

        private Transform[] _path;
        private int _nextPointIndex = -1;
        private bool _reverse;

        public bool WaitOnEveryPoint => _waitOnEveryPoint;
        public PathType PathType => _pathType;
        public Transform[] Path => _path ??= GetComponentsInChildren<Transform>().Except(transform).ToArray();
        public bool IsBusy { get; set; }

        public bool IsEndOfPath(Transform point)
        {
            return _path.Length > 0 && (_path[0] == point || _path[_path.Length - 1] == point);
        }
        
        public Transform Pop()
        {
            switch (PathType)
            {
                case PathType.Loop:
                    return GetNextLoopPoint();
                case PathType.PingPong:
                    return GetNextPingPongPoint();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private Transform GetNextLoopPoint()
        {
            _nextPointIndex++;
            if (_nextPointIndex >= _path.Length)
            {
                _nextPointIndex = 0;
            }
            return _path[_nextPointIndex];
        }

        private Transform GetNextPingPongPoint()
        {
            _nextPointIndex += _reverse ? -1 : 1;
            if (_nextPointIndex == 0)
            {
                _reverse = false;
            }
            if (_nextPointIndex == _path.Length - 1)
            {
                _reverse = true;
            }
            return _path[_nextPointIndex];
        }
    }
}
