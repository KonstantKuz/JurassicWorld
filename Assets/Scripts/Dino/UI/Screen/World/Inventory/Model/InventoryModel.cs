using System;
using System.Collections.Generic;
using System.Linq;
using Dino.Inventory.Model;
using Dino.Inventory.Service;
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
        public IReactiveProperty<List<ItemViewModel>> Items => _items;

        public Action<InventoryItem> OnClick;
        
        public InventoryModel(InventoryService inventoryService, ActiveItemService activeItemService)
        {
            _inventoryService = inventoryService;
            _activeItemService = activeItemService;
            UpdateItems();
            _inventoryService.InventoryProperty.Subscribe(it => UpdateItems());
        }

        public void UpdateItems()
        {
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

        private ItemViewModel CreateItemViewModel([CanBeNull] InventoryItem item)
        {
            return new ItemViewModel() {
                    Item = item,
                    State = GetState(item),
                    OnClick = () => OnClick?.Invoke(item)
            };
        }

        private ItemViewState GetState([CanBeNull] InventoryItem item)
        {
            if (item == null) {
                return ItemViewState.Empty;
            }
            if (_activeItemService.HasActiveItem() && _activeItemService.ActiveItemId.Value.Equals(item)) {
                return ItemViewState.Active;
            }
            return ItemViewState.Inactive;
        }
    }
}