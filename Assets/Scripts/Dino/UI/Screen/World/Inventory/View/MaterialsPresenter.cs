using Dino.Inventory.Service;
using Dino.UI.Screen.World.Inventory.Model;
using Feofun.UI.Components;
using UnityEngine;
using Zenject;

namespace Dino.UI.Screen.World.Inventory.View
{
    public class MaterialsPresenter : MonoBehaviour
    {
        [SerializeField] 
        private ListView _view;

        private MaterialsModel _model;
        
        [Inject]
        private InventoryService _inventoryService;

        private void OnEnable()
        {
            Dispose();
            Init();
        }

        private void Init()
        {
            _model = new MaterialsModel(_inventoryService);
            _view.Init(_model.Materials);
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