using System;
using System.Collections.Generic;
using System.Linq;
using Dino.Inventory.Model;
using Dino.Inventory.Service;
using UniRx;

namespace Dino.UI.Screen.World.Inventory.Model
{
    public class MaterialsModel
    {
        private readonly ReactiveProperty<List<MaterialViewModel>> _materials = new ReactiveProperty<List<MaterialViewModel>>();
        private readonly InventoryService _inventoryService;
        
        private IDisposable _disposable;
        
        public IReactiveProperty<List<MaterialViewModel>> Materials => _materials;

        public MaterialsModel(InventoryService inventoryService)
        {
            _inventoryService = inventoryService;
            _disposable = inventoryService.InventoryProperty.Subscribe(it => UpdateMaterials());
        }

        private void UpdateMaterials()
        {
            _materials.SetValueAndForceNotify(CreateMaterials());
        }

        private List<MaterialViewModel> CreateMaterials()
        {
            return _inventoryService.GetItems(InventoryItemType.Material).Select(it => new MaterialViewModel(it)).ToList();
        }

        public void Dispose()
        {
            _disposable?.Dispose();
            _disposable = null;
        }
    }
}