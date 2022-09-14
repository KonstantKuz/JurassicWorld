using System;
using UnityEngine;

namespace Dino.Location.Workbench
{
    public class CraftTimer : IDisposable
    {
        public float Duration { get; }
        public float Progress { get; private set; }
        
        private Action _onCraft;

        public CraftTimer(float duration, Action onCraft)
        {
            _onCraft = onCraft;
            Duration = duration;
            Progress = 0;
        }
        
        public void IncreaseProgress()
        {
            Progress += Time.deltaTime;
            if (Progress <= Duration) return;
            _onCraft?.Invoke();
            _onCraft = null;
        }
        public void Dispose()
        {
            _onCraft = null; 
        }
    }
}