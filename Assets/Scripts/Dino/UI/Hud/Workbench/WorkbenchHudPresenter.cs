using System;
using Dino.Inventory.Service;
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


        private CompositeDisposable _disposable;
        private WorkbenchHudModel _model;

        public void Init(Location.Workbench.Workbench workbench)
        {
            Dispose();
            _disposable = new CompositeDisposable();
            
            _model = new WorkbenchHudModel(workbench, OnCraft);
            _view.Init(_model);
            _inventoryService.InventoryProperty.Subscribe(it => OnModelUpdate()).AddTo(_disposable);
            workbench.OnPlayerTriggered += OnModelUpdate;

        }
        private void OnCraft()
        {
            if (!_model.Workbench.CanCraftRecipe()) {
                this.Logger().Error($"Recipe crafting error, invalid ingredients, recipeId:= {_model.Workbench.CraftItemName}");
                return;
            }
            _model.Workbench.Craft();
        }

        private void Dispose()
        {
            _disposable?.Dispose();
            _disposable = null;
            if (_model != null) {
                _model.Workbench.OnPlayerTriggered -= OnModelUpdate;
            }
            _model = null;
        }

        private void OnModelUpdate() => _model.Update();

        private void OnDestroy()
        {
            Dispose();
        }
    }
}