using System;
using System.Collections.Generic;
using System.Linq;
using Dino.Inventory.Config;
using Dino.Inventory.Model;
using Dino.Inventory.Service;
using Dino.Units.Service;
using Dino.Weapon.Service;
using JetBrains.Annotations;
using UniRx;

namespace Dino.UI.Screen.World.Inventory.Model
{
    public class InventoryModel
    {
        private readonly ReactiveProperty<List<ItemViewModel>> _items = new ReactiveProperty<List<ItemViewModel>>();

        private readonly InventoryService _inventoryService;
        private readonly ActiveItemService _activeItemService;
        private readonly CraftService _craftService;
        private readonly WeaponService _weaponService;
        private readonly Action<Item> _onClick;
        private readonly Action<ItemViewModel> _onBeginDrag;
        private readonly Action<ItemViewModel> _onEndDrag;

        private List<CraftRecipeConfig> _allPossibleRecipes = new List<CraftRecipeConfig>();
        private CompositeDisposable _disposable;
        
        public readonly bool IsDropEnabled;    
        public readonly bool IsCraftEnabled;
        public readonly InventoryItemType InventoryType;
        public IReactiveProperty<List<ItemViewModel>> Items => _items;
        
        public InventoryModel(InventoryItemType inventoryType,InventoryService inventoryService,
                              ActiveItemService activeItemService,
                              CraftService craftService,
                              WeaponService weaponService,
                              UiInventorySettings uiInventorySettings,
                              Action<Item> onClick,
                              Action<ItemViewModel> onBeginDrag,
                              Action<ItemViewModel> onEndDrag)
        {
            _disposable = new CompositeDisposable();
            InventoryType = inventoryType;
            _inventoryService = inventoryService;
            _activeItemService = activeItemService;
            _craftService = craftService;
            _weaponService = weaponService;
            _onClick = onClick;
            _onBeginDrag = onBeginDrag;
            _onEndDrag = onEndDrag;
            IsDropEnabled = uiInventorySettings.IsDropEnabled;
            IsCraftEnabled = uiInventorySettings.IsCraftEnabled;
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
                return Enumerable.Repeat(ItemViewModel.Empty(), InventoryService.MAX_UNIQUE_WEAPONS_COUNT).ToList();
            }
            var items = _inventoryService.GetItems(InventoryType).ToList();
            return items.Select(CreateItemViewModel)
                        .Concat(Enumerable.Repeat(ItemViewModel.Empty(), InventoryService.MAX_UNIQUE_WEAPONS_COUNT - items.Count))
                        .ToList();
        }

        private ItemViewModel CreateItemViewModel(Item item)
        {
            return new ItemViewModel(item, GetState(item), CanCraft(item), _weaponService.FindWeaponWrapper(item.Id), () => _onClick?.Invoke(item), _onBeginDrag,
                                     _onEndDrag);
        }

        public void UpdateItemModel(ItemViewModel model)
        {
            model.UpdateState(GetState(model.Item));
            model.UpdateCraftState(CanCraft(model.Item));
        }

        private bool CanCraft(Item item)
        {
            return IsCraftEnabled && _allPossibleRecipes.Any(recipe => recipe.ContainsIngredient(item.Id.FullName));
        }

        private ItemViewState GetState([CanBeNull] Item item)
        {
            if (item == null) {
                return ItemViewState.Empty;
            }
            return _activeItemService.IsActiveItem(item.Id) ? ItemViewState.Active : ItemViewState.Inactive;
        }
    }
}