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
            model.CraftProgress.Subscribe(progress => _progressBar.Reset(progress)).AddTo(_disposable);
            model.IsCrafting.Subscribe(isCrafting => _progressBarRoot.gameObject.SetActive(isCrafting)).AddTo(_disposable);
        }

        private void LoadIcon(WorkbenchHudModel model)
        {
            _icon.sprite = Resources.Load<Sprite>(model.Icon);
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