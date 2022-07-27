using System;
using UnityEngine;

namespace Dino.Units.StateMachine.States
{
    public class PatrolPathIterator : MonoBehaviour
    {
        [SerializeField] private PatrolBehaviourType _patrolBehaviourType;

        private int _currentPointIndex;
        private bool _reverse;

        public int CurrentPointIndex => _currentPointIndex;
        
        public void IncreaseCurrentIndex(int pathLength)
        {
            switch (_patrolBehaviourType)
            {
                case PatrolBehaviourType.Loop:
                    IncreaseCurrentIndexLoop(pathLength);
                    break;
                case PatrolBehaviourType.PingPong:
                    IncreaseCurrentIndexPingPong(pathLength);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void IncreaseCurrentIndexLoop(int pathLength)
        {
            _currentPointIndex++;
            if (_currentPointIndex >= pathLength)
            {
                _currentPointIndex = 0;
            }
        }

        private void IncreaseCurrentIndexPingPong(int pathLength)
        {
            _currentPointIndex += _reverse ? -1 : 1;
            if (_currentPointIndex == 0)
            {
                _reverse = false;
            }
            if (_currentPointIndex == pathLength - 1)
            {
                _reverse = true;
            }
        }
    }
}