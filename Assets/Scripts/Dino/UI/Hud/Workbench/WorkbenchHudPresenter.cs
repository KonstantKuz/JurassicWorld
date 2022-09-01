using Dino.Inventory.Service;
using Dino.Units.Service;
using Logger.Extension;
using UniRx;
using UnityEngine;
using Zenject;

namespace Dino.UI.Hud.Workbench
{
    public class WorkbenchHudPresenter : MonoBehaviour
    {
        [SerializeField] private WorkbenchHudView _view;

        [Inject]
        private InventoryService _inventoryService;
        [Inject]
        private CraftService _craftService;
        [Inject]
        private ActiveItemService _activeItemService;

        private CompositeDisposable _disposable;
        
        private WorkbenchHudModel _model;

        public void Init(Location.Workbench.Workbench workbench)
        {
            Dispose();
            _disposable = new CompositeDisposable();
            
            _model = new WorkbenchHudModel(workbench, _craftService, OnCraft);
            _view.Init(_model);
            _inventoryService.InventoryProperty.Subscribe(it => OnModelUpdate()).AddTo(_disposable);
            workbench.OnPlayerTriggered += OnModelUpdate;

        }
        private void OnCraft()
        {
            var recipe = _craftService.FindHighestRankPossibleRecipeBy(_model.CraftItemName);
            if (recipe == null) {
                this.Logger().Error($"Recipe crafting error, missing ingredients, craftItemName:= {_model.CraftItemName}");
                return;
            }
            var item = _craftService.Craft(recipe.CraftItemId);
            _activeItemService.Replace(item);
        }
        private void OnModelUpdate() => _model.Update();
        private void Dispose()
        {
            _disposable?.Dispose();
            _disposable = null;
            if (_model != null) {
                _model.Workbench.OnPlayerTriggered -= OnModelUpdate;
            }
            _model = null;
        }
        private void OnDestroy()
        {
            Dispose();
        }
    }
}