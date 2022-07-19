using Dino.Inventory.Service;
using Dino.UI.Screen.World.Inventory.Model;
using Dino.UI.Screen.World.Inventory.View;
using UnityEngine;
using Zenject;

namespace Dino.UI.Screen.World.Inventory
{
    public class InventoryPresenter : MonoBehaviour
    {

        [SerializeField]
        private InventoryView _view;
        
        
        [Inject] private InventoryService _inventoryService;
        [Inject] private ActiveItemService _activeItemService;

        private InventoryModel _model;

        private void OnEnable()
        {
            _model = new InventoryModel(_inventoryService, _activeItemService);
            _view.Init(_model.Items);
        }

        private void OnDisable()
        {
            _model = null;
        }
    }
}