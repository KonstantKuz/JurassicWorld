using Feofun.UI.Components;
using UniRx;
using UnityEngine;

namespace Dino.UI.Hud.Unit
{
    public class HealthBarView : MonoBehaviour
    {
        [SerializeField]
        private ProgressBarView _progressBar;

        private CompositeDisposable _disposable;
        
        public void Init(HealthBarModel model)
        {
            _disposable?.Dispose();
            _disposable = new CompositeDisposable();
            model.Percent.Subscribe(UpdateProgressBar).AddTo(_disposable);
        }
        private void UpdateProgressBar(float value)
        {
            _progressBar.SetData(value);
        }
        protected void OnDisable()
        {
            _disposable?.Dispose();
            _disposable = null;
        }
    }
}