﻿using System;
using System.Collections.Generic;
using System.Linq;
using Dino.Inventory.Config;
using Dino.Inventory.Model;
using Dino.Inventory.Service;
using Dino.Units.Service;
using JetBrains.Annotations;
using Logger.Extension;
using UniRx;

namespace Dino.UI.Screen.World.Inventory.Model
{
    public class InventoryModel
    {
        private readonly ReactiveProperty<List<ItemViewModel>> _items = new ReactiveProperty<List<ItemViewModel>>();

        private readonly InventoryService _inventoryService;
        private readonly ActiveItemService _activeItemService;
        private readonly CraftService _craftService;
        
        private readonly Action<ItemId> _onClick; 
        private readonly Action<ItemViewModel> _onBeginDrag;    
        private readonly Action<ItemViewModel> _onEndDrag;
        
        private CompositeDisposable _disposable;

        private List<CraftRecipeConfig> _allPossibleRecipes = new List<CraftRecipeConfig>();
        public IReactiveProperty<List<ItemViewModel>> Items => _items;
        
        public InventoryModel(InventoryService inventoryService, ActiveItemService activeItemService, CraftService craftService, Action<ItemId> onClick,
                              Action<ItemViewModel> onBeginDrag,
                              Action<ItemViewModel> onEndDrag)
        {
            _disposable = new CompositeDisposable();
            _inventoryService = inventoryService;
            _activeItemService = activeItemService;
            _craftService = craftService;
            _onClick = onClick;
            _onBeginDrag = onBeginDrag;
            _onEndDrag = onEndDrag;
            UpdateModel();
            _inventoryService.InventoryProperty.Select(it => new Unit())
                             .Merge(_activeItemService.ActiveItemId.Select(it => new Unit()))
                             .Subscribe(it => UpdateModel())
                             .AddTo(_disposable);
        }

        private void UpdateModel()
        {
            _allPossibleRecipes = _craftService.GetAllPossibleRecipes().ToList(); 
            _items.SetValueAndForceNotify(CreateItems());
        }
        public void Dispose()
        {
            _disposable?.Dispose();
            _disposable = null;
        }
        private List<ItemViewModel> CreateItems()
        {
            if (!_inventoryService.HasInventory()) {
                return Enumerable.Repeat(ItemViewModel.Empty(), InventoryService.MAX_ITEMS_COUNT).ToList();
            }
            var items = _inventoryService.InventoryProperty.Value.Items;

            if (items.Count > InventoryService.MAX_ITEMS_COUNT) {
                this.Logger().Warn($"The number of items in the inventory is more than VISIBLE_ITEM_COUNT:= {InventoryService.MAX_ITEMS_COUNT}");
                return items.Take(InventoryService.MAX_ITEMS_COUNT).Select(CreateItemViewModel).ToList();
            }
            return items.Select(CreateItemViewModel).Concat(Enumerable.Repeat(ItemViewModel.Empty(), InventoryService.MAX_ITEMS_COUNT - items.Count)).ToList();
        }

        private ItemViewModel CreateItemViewModel(ItemId id)
        {
            return new ItemViewModel(id, GetState(id), CanCraft(id), () => _onClick?.Invoke(id), _onBeginDrag, _onEndDrag);
        }

        public void UpdateItemModel(ItemViewModel model)
        {
            model.UpdateState(GetState(model.Id));     
            model.UpdateCraftState(CanCraft(model.Id));
        }

        private bool CanCraft(ItemId id)
        {
            return _allPossibleRecipes.Any(recipe => recipe.ContainsIngredient(id.FullName));
        }

        private ItemViewState GetState([CanBeNull] ItemId id)
        {
            if (id == null) {
                return ItemViewState.Empty;
            }
            if (_activeItemService.HasActiveItem() && _activeItemService.ActiveItemId.Value.Equals(id)) {
                return ItemViewState.Active;
            }
            return ItemViewState.Inactive;
        }
    }
}