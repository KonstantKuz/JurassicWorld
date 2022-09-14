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
        
        private CompositeDisposable _disposable;
        public void Init(WorkbenchHudModel model)
        {
            Dispose();
            _disposable = new CompositeDisposable();

            LoadIcon(model);
            _button.Init(model.OnCraft);
            model.CraftButtonShown.Subscribe(SetButtonActive).AddTo(_disposable);    
            model.CraftAvailable.Subscribe(UpdateAvailableState).AddTo(_disposable);
        }
        private void LoadIcon(WorkbenchHudModel model)
        {
            _icon.sprite = Resources.Load<Sprite>(IconPath.GetInventory(model.CraftItemName));
        }
        private void SetButtonActive(bool shown)
        {
            _button.gameObject.SetActive(shown);
           
        }   
        private void UpdateAvailableState(bool available)
        {
            _button.Button.interactable = available;
            _icon.color = available ? _availableCraftColor : _notAvailableCraftColor;
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