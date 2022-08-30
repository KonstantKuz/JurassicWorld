using Feofun.UI.Components.Button;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Dino.UI.Hud.Workbench
{
    public class WorkbenchHudView : MonoBehaviour
    {
        [SerializeField] private ActionButton _button;     
        
        [SerializeField] private Image _icon;

        private WorkbenchHudModel _model;

        private CompositeDisposable _disposable;
        public void Init(WorkbenchHudModel model)
        {
            Dispose();
            _disposable = new CompositeDisposable();
            
            _button.Init(model.OnCraft);
            model.CraftButtonShown.Subscribe(it => OnButtonUpdate()).AddTo(_disposable);
        }

        private void OnButtonUpdate()
        {
            _button.gameObject.SetActive(_model.CraftButtonShown.Value);
            _button.Button.interactable = _model.CanCraft.Value;
        }


        private void Dispose()
        {
            _disposable?.Dispose();
            _disposable = null;
            _model = null;
        }

        private void OnDestroy()
        {
            Dispose();
        }
    }
}