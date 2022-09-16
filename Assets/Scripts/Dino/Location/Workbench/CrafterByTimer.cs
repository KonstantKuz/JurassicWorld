using System;
using Dino.Extension;
using Dino.Inventory.Extension;
using Dino.Inventory.Service;
using JetBrains.Annotations;
using Logger.Extension;
using SuperMaxim.Messaging;
using UniRx;
using UnityEngine;

namespace Dino.Location.Workbench
{
    public class CrafterByTimer : IDisposable
    {
        private readonly ReactiveProperty<bool> _hasActiveTimer = new ReactiveProperty<bool>(false);
        private readonly CraftService _craftService;
        private readonly IMessenger _messenger;

        private readonly string _craftItemId;
        private readonly float _craftDuration;
        private readonly Vector3 _craftItemPosition;

        private CompositeDisposable _disposable;

        [CanBeNull]
        private ActionTimer _craftTimer;

        public ActionTimer CraftTimer => _craftTimer;
        
        public IReactiveProperty<bool> HasActiveTimer => _hasActiveTimer;

        public CrafterByTimer(InventoryService inventoryService, 
                              CraftService craftService,
                              IMessenger messanger,
                              string craftItemId, 
                              float craftDuration, 
                              Vector3 craftItemPosition)
        {
            Dispose();
            _disposable = new CompositeDisposable();
            _craftService = craftService;
            _messenger = messanger;
            _craftItemId = craftItemId;
            _craftDuration = craftDuration;
            _craftItemPosition = craftItemPosition;
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
            var recipeConfig = _craftService.GetRecipeConfig(_craftItemId);
            var item = _craftService.Craft(recipeConfig.CraftItemId);
            item.TryPublishReceivedLoot(_messenger, recipeConfig.CraftItem.Count, _craftItemPosition.WorldToScreenPoint());
        }

        public void Dispose()
        {
            _disposable?.Dispose();
            _disposable = null;
            DeleteTimer();
        }
    }
}