using System;
using UniRx;
using UnityEngine;

namespace Dino.Location.Workbench
{
    public class ActionTimer : IDisposable
    {
        private readonly ReactiveProperty<float> _progress = new ReactiveProperty<float>();
        public float Duration { get; }

        private Action _onComplete;
        
        public IReactiveProperty<float> Progress => _progress;

        public ActionTimer(float duration, Action onComplete)
        {
            _onComplete = onComplete;
            Duration = duration;
            _progress.Value = 0;
        }
        
        public void IncreaseProgress()
        {
            Progress.Value += Time.deltaTime;
            if (Progress.Value <= Duration) return;
            _onComplete?.Invoke();
            Dispose();
        }
        public void Dispose()
        {
            _onComplete = null;
            _progress.Dispose();
        }
    }
}