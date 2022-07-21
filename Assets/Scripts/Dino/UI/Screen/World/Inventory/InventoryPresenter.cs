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

        private InventoryModel _model;

        private void OnEnable()
        {
            _model = new InventoryModel(_inventoryService, _activeItemService, UpdateActiveItem, OnBeginDrag, OnEndDrag);
            _view.Init(_model.Items);
        }

        private void OnEndDrag(ItemViewModel model)
        {
            
            _itemCursor.Detach();
        }

        private void OnBeginDrag(ItemViewModel model)
        {
            var dragItem = Instantiate(_itemPrefab);
            var newModel = new ItemViewModel();
            newModel.Id = model.Id;
            dragItem.InitDragView(newModel);
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