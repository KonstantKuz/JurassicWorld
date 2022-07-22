using System;
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
        private const int VISIBLE_ITEM_COUNT = 4;
        
        private readonly ReactiveProperty<List<ItemViewModel>> _items = new ReactiveProperty<List<ItemViewModel>>();
        
        
        private readonly InventoryService _inventoryService;
        private readonly ActiveItemService _activeItemService;
        private readonly CraftService _craftService;

        private List<CraftRecipeConfig> _allPossibleRecipes = new List<CraftRecipeConfig>();
        public IReactiveProperty<List<ItemViewModel>> Items => _items;

        private Action<ItemId> _onClick; 
        private Action<ItemViewModel> _onBeginDrag;    
        private Action<ItemViewModel> _onEndDrag;
        
        public InventoryModel(InventoryService inventoryService, ActiveItemService activeItemService, CraftService craftService, Action<ItemId> onClick,
                              Action<ItemViewModel> onBeginDrag,
                              Action<ItemViewModel> onEndDrag)
        {
            _inventoryService = inventoryService;
            _activeItemService = activeItemService;
            _craftService = craftService;
            _onClick = onClick;
            _onBeginDrag = onBeginDrag;
            _onEndDrag = onEndDrag;
            UpdateItems();
            _inventoryService.InventoryProperty.Subscribe(it => UpdateItems());   
            _activeItemService.ActiveItemId.Subscribe(it => UpdateItems());
        }

        public void UpdateItems()
        {
            _allPossibleRecipes = _craftService.GetAllPossibleRecipes().ToList(); // todo
            _items.SetValueAndForceNotify(CreateItems());
        }

        private List<ItemViewModel> CreateItems()
        {
            if (!_inventoryService.HasInventory()) {
                return Enumerable.Repeat(ItemViewModel.Empty(), VISIBLE_ITEM_COUNT).ToList();
            }
            var items = _inventoryService.InventoryProperty.Value.Items;

            if (items.Count > VISIBLE_ITEM_COUNT) {
                this.Logger().Warn($"The number of items in the inventory is more than VISIBLE_ITEM_COUNT:= {VISIBLE_ITEM_COUNT}");
                return items.Take(VISIBLE_ITEM_COUNT).Select(CreateItemViewModel).ToList();
            }
            return items.Select(CreateItemViewModel).Concat(Enumerable.Repeat(ItemViewModel.Empty(), VISIBLE_ITEM_COUNT - items.Count)).ToList();
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
            return _allPossibleRecipes.Any(recipe => recipe.ContainsIngredient(id.Name));
        }

        public ItemViewState GetState([CanBeNull] ItemId id)
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