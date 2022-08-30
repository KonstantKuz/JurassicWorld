using Dino.Inventory.Model;
using Dino.Util;
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

        [SerializeField]
        private Color _availableCraftColor; 
        [SerializeField]
        private Color _notAvailableCraftColor;
        
        private WorkbenchHudModel _model;
        private CompositeDisposable _disposable;
        public void Init(WorkbenchHudModel model)
        {
            Dispose();
            _disposable = new CompositeDisposable();
            _model = model;
            
            LoadIcon();
            _button.Init(model.OnCraft);
            model.CraftButtonShown.Subscribe(it => UpdateButtonState()).AddTo(_disposable);    
            model.CraftAvailable.Subscribe(it => UpdateIconState()).AddTo(_disposable);
        }
        private void LoadIcon()
        {
            _icon.sprite = Resources.Load<Sprite>(IconPath.GetInventory(_model.CraftItemName));
        }
        private void UpdateButtonState()
        {
            _button.gameObject.SetActive(_model.CraftButtonShown.Value);
            _button.Button.interactable = _model.CraftAvailable.Value;
        }   
        private void UpdateIconState()
        {
            _icon.color = _model.CraftAvailable.Value ? _availableCraftColor : _notAvailableCraftColor;
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