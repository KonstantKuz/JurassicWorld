using System.Collections.Generic;
using Dino.Inventory.Model;
using Dino.Inventory.Service;
using Dino.UI.Screen.World.Inventory.Model;
using Dino.UI.Screen.World.Inventory.View;
using Dino.Units.Service;
using UnityEngine;
using Zenject;

namespace Dino.UI.Screen.World.Inventory
{
    public class InventoryPresenter : MonoBehaviour
    {
        [SerializeField]
        private InventoryView _view;      
        [SerializeField]
        private ItemCursor _itemCursor;
        [SerializeField]
        private InventoryItemView _itemPrefab;  
        
        [Inject] private InventoryService _inventoryService;
        [Inject] private ActiveItemService _activeItemService;     
        [Inject] private CraftService _craftService;

        private InventoryModel _model;

        private void OnEnable()
        {
            _model = new InventoryModel(_inventoryService, _activeItemService, _craftService, UpdateActiveItem, OnBeginDrag, OnEndDrag);
            _view.Init(_model.Items);
        }

        private void OnEndDrag(ItemViewModel model)
        {
            var secondItemModel = _itemCursor.FirstComponentByMousePosition<InventoryItemView>();
            if (secondItemModel != null && secondItemModel.Model != null && secondItemModel.Model.State.Value != ItemViewState.Empty) {
                TryCraft(model, secondItemModel.Model);
                return;
            }
            _model.UpdateItemModel(model);
            _itemCursor.Detach();
        }

        private void TryCraft(ItemViewModel firstItemModel, ItemViewModel secondItemModel)
        {
            var ingredients = new HashSet<ItemId>() {
                    firstItemModel.Id,
                    secondItemModel.Id
            };
            var recipe = _craftService.FindFirstPossibleRecipe(ingredients);
            if (recipe == null) {
                _model.UpdateItemModel(firstItemModel);
                _itemCursor.Detach();
                return;
            }
            _craftService.Craft(ingredients);
            _itemCursor.Detach();
        }

        private void OnBeginDrag(ItemViewModel model)
        {
            var dragItem = Instantiate(_itemPrefab);
            var newModel = ItemViewModel.ForDrag(model.Id, model.CanCraft.Value);
            
            
            model.UpdateState(ItemViewState.Empty); 
            model.UpdateCraftState(false);
            
      
            dragItem.InitViewForDrag(newModel);
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
        private void OnDisable()
        {
            _model = null;
        }
    }
}