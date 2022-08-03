using System.Collections.Generic;
using Dino.Inventory.Model;
using Dino.Inventory.Service;
using Dino.Loot.Service;
using Dino.UI.Screen.World.Inventory.Model;
using Dino.UI.Screen.World.Inventory.View;
using Dino.Units.Service;
using UnityEngine;
using Zenject;

namespace Dino.UI.Screen.World.Inventory
{
    public class InventoryPresenter : MonoBehaviour
    {
        private const string INVENTORY_LAYER_NAME = "Inventory";
        
        [SerializeField]
        private InventoryView _view;      
        [SerializeField]
        private ItemCursor _itemCursor;

        [Inject] private InventoryService _inventoryService;
        [Inject] private ActiveItemService _activeItemService;     
        [Inject] private CraftService _craftService;     
        [Inject] private LootService _lootService;
        [Inject] private DiContainer _container;
        
        private InventoryModel _model;

        private void OnEnable()
        {
            Dispose();
            _model = new InventoryModel(_inventoryService, _activeItemService, _craftService, UpdateActiveItem, OnBeginItemDrag, OnEndItemDrag);
            _view.Init(_model.Items);
        }

        private void OnEndItemDrag(ItemViewModel model)
        {
            var secondItemModel = _itemCursor.FindComponentUnderCursor<InventoryItemView>();
   
            if (secondItemModel != null && secondItemModel.Model != null && secondItemModel.Model.State.Value != ItemViewState.Empty) {
                TryCraft(model, secondItemModel.Model);
                return;
            }
            if (_itemCursor.IsCursorOverLayer(INVENTORY_LAYER_NAME)) {
                _model.UpdateItemModel(model);
            } else {
                RemoveItemFromInventory(model);
            }
            _itemCursor.Detach();
        }

        private void RemoveItemFromInventory(ItemViewModel itemModel)
        {
            if (_activeItemService.IsActiveItem(itemModel.Id)) {
                _activeItemService.UnEquip();
            }
            _inventoryService.Remove(itemModel.Id);
            _lootService.DropLoot(itemModel.Id);
        }

        private void TryCraft(ItemViewModel firstItemModel, ItemViewModel secondItemModel)
        {
            var ingredients = new HashSet<ItemId>() {
                    firstItemModel.Id,
                    secondItemModel.Id
            };
            var recipe = _craftService.FindFirstMatchingRecipe(ingredients);
            if (recipe == null) {
                _model.UpdateItemModel(firstItemModel);
                _itemCursor.Detach();
                return;
            }
            var craftedItem = _craftService.Craft(ingredients);
            if (craftedItem.Rank >= _activeItemService.ActiveItemId.Value.Rank)
            {
                _activeItemService.Replace(craftedItem);
            }
            _itemCursor.Detach();
        }

        private void OnBeginItemDrag(ItemViewModel model)
        {
            var dragItem = _container.InstantiatePrefabForComponent<InventoryItemView>(_view.ItemPrefab);
            var dragModel = ItemViewModel.ForDrag(model.Id, model.CanCraft.Value);
            
            model.UpdateState(ItemViewState.Empty); 
            model.UpdateCraftState(false);
            
            dragItem.InitViewForDrag(dragModel);
            _itemCursor.Attach(dragItem.gameObject);
        }
        
        private void UpdateActiveItem(ItemId itemId)
        {
            if (!_activeItemService.HasActiveItem()) {
                _activeItemService.Equip(itemId);
                return;
            }
            if (_activeItemService.ActiveItemId.Value.Equals(itemId)) {
                _activeItemService.UnEquip();
                return;
            }
            _activeItemService.Replace(itemId);
        }

        private void Dispose()
        {
            _model?.Dispose();
            _model = null;
        }

        private void OnDisable()
        {
            Dispose();
        }
    }
}