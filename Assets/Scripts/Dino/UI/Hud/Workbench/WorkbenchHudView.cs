using Feofun.UI.Components;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Dino.UI.Hud.Workbench
{
    public class WorkbenchHudView : MonoBehaviour
    {
        [SerializeField] 
        private Image _icon;
        [SerializeField] 
        private ProgressBarView _progressBar;
        [SerializeField] 
        private Transform _progressBarRoot;
        
        private CompositeDisposable _disposable;
        
        public void Init(WorkbenchHudModel model)
        {
            Dispose();
            _disposable = new CompositeDisposable();
            LoadIcon(model);
            model.CraftProgress.Subscribe(OnProgressUpdate).AddTo(_disposable);
        }
        private void LoadIcon(WorkbenchHudModel model)
        {
            _icon.sprite = Resources.Load<Sprite>(model.Icon);
        }
        private void OnProgressUpdate(CraftProgress progress)
        {
            _progressBarRoot.gameObject.SetActive(progress.Enabled);
            _progressBar.Reset(progress.Progress);
        }
        private void Dispose()
        {
            _disposable?.Dispose();
            _disposable = null;
        }
        private void OnDestroy()
        {
            Dispose();
        }
    }
}