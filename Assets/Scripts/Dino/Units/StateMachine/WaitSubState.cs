using System;
using UnityEngine;

namespace Dino.Units.StateMachine
{
    public class WaitSubState
    {
        private readonly Action _onEnter;
        private readonly Action _onTick;
        private readonly Action _onExit;
        private float _timeToWait;
        private float _waitTimer;

        public static WaitSubState Build(float timeToWait, Action onEnter, Action onTick, Action onExit)
        {
            var state = new WaitSubState(onEnter, onTick, onExit);
            state.SetTimeToWait(timeToWait);
            return state;
        }

        private WaitSubState(Action onEnter, Action onTick, Action onExit)
        {
            _onEnter = onEnter;
            _onTick = onTick;
            _onExit = onExit;
        }

        public void SetTimeToWait(float timeToWait)
        {
            _timeToWait = timeToWait;
        }
        
        public void OnTick()
        {
            if (_waitTimer == 0f)
            {
                _onEnter?.Invoke();
            }
            _waitTimer += Time.deltaTime;
            if (_waitTimer >= _timeToWait)
            {
                _waitTimer = 0f;
                _onExit?.Invoke();
                return;
            }
            _onTick?.Invoke();
        }
    }
}