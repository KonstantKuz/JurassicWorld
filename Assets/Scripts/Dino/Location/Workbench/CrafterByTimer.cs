using System;
using Dino.Inventory.Service;
using JetBrains.Annotations;
using Logger.Extension;
using UniRx;

namespace Dino.Location.Workbench
{
    public class CrafterByTimer : IDisposable
    {

        private readonly ReactiveProperty<bool> _hasActiveTimer = new ReactiveProperty<bool>(false);
        
        private readonly CraftService _craftService;

        private readonly string _craftItemId;
        private readonly float _craftDuration;

        private CompositeDisposable _disposable;

        [CanBeNull]
        private ActionTimer _craftTimer;

        public ActionTimer CraftTimer => _craftTimer;
        
        public IReactiveProperty<bool> HasActiveTimer => _hasActiveTimer;

        public CrafterByTimer(InventoryService inventoryService, 
                              CraftService craftService, 
                              string craftItemId, float craftDuration)
        {
            Dispose();
            _disposable = new CompositeDisposable();
            _craftService = craftService;
            _craftItemId = craftItemId;
            _craftDuration = craftDuration;
            inventoryService.InventoryProperty.Subscribe(it => OnInventoryUpdated()).AddTo(_disposable);
        }

        public void Update() => _craftTimer?.IncreaseProgress();

        private void OnInventoryUpdated()
        {
            if (_craftTimer == null && CanCraft()) {
                CreateTimer();
            }
            if (_craftTimer != null && !CanCraft()) {
                DeleteTimer();
            }
        }

        private void CreateTimer()
        {
            DeleteTimer();
            _craftTimer = new ActionTimer(_craftDuration, OnCraft);
            _hasActiveTimer.SetValueAndForceNotify(true);
        }

        private void DeleteTimer()
        {
            _craftTimer?.Dispose();
            _craftTimer = null;
            _hasActiveTimer.SetValueAndForceNotify(false);
        }

        private bool CanCraft() => _craftService.HasIngredientsForRecipe(_craftItemId);

        private void OnCraft()
        {
            if (!CanCraft()) {
                this.Logger().Warn($"Recipe crafting error, missing ingredients, craftItemId:= {_craftItemId}");
                return;
            }
            DeleteTimer();
            _craftService.Craft(_craftItemId);
        }

        public void Dispose()
        {
            _disposable?.Dispose();
            _disposable = null;
            DeleteTimer();
        }
    }
}