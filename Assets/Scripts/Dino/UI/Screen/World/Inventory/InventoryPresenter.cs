using System.Collections.Generic;
using Dino.Inventory.Model;
using Dino.Inventory.Service;
using Dino.Loot.Service;
using Dino.UI.Screen.World.Inventory.Model;
using Dino.UI.Screen.World.Inventory.View;
using Dino.Units.Service;
using Dino.Weapon.Service;
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
        [SerializeField]
        private InventoryItemType _inventoryType = InventoryItemType.Weapon;

        [Inject] private InventoryService _inventoryService;
        [Inject] private ActiveItemService _activeItemService;     
        [Inject] private CraftService _craftService;     
        [Inject] private LootService _lootService;
        [Inject] private DiContainer _container;
        [Inject] private WeaponService _weaponService;
        [Inject] private UiInventorySettings _uiInventorySettings;
        [Inject] private Feofun.ABTest.ABTest _abTest;
        private InventoryModel _model;

        private void OnEnable()
        {
            Dispose();
            _model = new InventoryModel(_inventoryType,
                                        _inventoryService, 
                                        _activeItemService, 
                                        _craftService, 
                                        _weaponService, 
                                        _uiInventorySettings, 
                                        _abTest,
                                        UpdateActiveItem, 
                                        OnBeginItemDrag, 
                                        OnEndItemDrag); 
            _view.Init(_model);
        }

        private void OnEndItemDrag(ItemViewModel model)
        {
            var secondItemModel = _itemCursor.FindComponentUnderCursor<InventoryItemView>();
   
            if (CanCraft(secondItemModel)) {
                TryCraft(model, secondItemModel.Model);
                return;
            }
            if (_itemCursor.IsCursorOverLayer(INVENTORY_LAYER_NAME)) {
                _model.UpdateItemModel(model);
            } 
            else if (_model.IsDropEnabled) {
                RemoveItemFromInventory(model);
            } else {
                _model.UpdateItemModel(model);
            }
            _itemCursor.Detach();
        }

        private bool CanCraft(InventoryItemView secondItemModel) =>
                secondItemModel != null && secondItemModel.Model != null && secondItemModel.Model.State.Value != ItemViewState.Empty && _model.IsCraftEnabled;

        private void RemoveItemFromInventory(ItemViewModel itemModel)
        {
            _lootService.DropLoot(itemModel.Item);
            _inventoryService.Remove(itemModel.Item.Id);
        }

        private void TryCraft(ItemViewModel firstItemModel, ItemViewModel secondItemModel)
        {
            var ingredients = new HashSet<Item>() {
                    firstItemModel.Item,
                    secondItemModel.Item
            };
            var recipe = _craftService.FindFirstMatchingRecipe(ingredients);
            if (recipe == null) {
                _model.UpdateItemModel(firstItemModel);
                _itemCursor.Detach();
                return;
            }
            _craftService.Craft(ingredients);
            _itemCursor.Detach();
        }

        private void OnBeginItemDrag(ItemViewModel model)
        {
            var dragItem = _container.InstantiatePrefabForComponent<InventoryItemView>(_view.ItemPrefab);
            var dragModel = ItemViewModel.ForDrag(model.Item, model.CanCraft.Value);
            
            model.UpdateState(ItemViewState.Empty); 
            model.UpdateCraftState(false);
            
            dragItem.InitViewForDrag(dragModel);
            _itemCursor.Attach(dragItem.gameObject);
        }
        
        private void UpdateActiveItem(Item item)
        {
            if (!_activeItemService.HasActiveItem()) {
                _activeItemService.Equip(item);
                return;
            }
            if (_activeItemService.ActiveItemId.Value.Equals(item)) {
                _activeItemService.UnEquip();
                return;
            }
            _activeItemService.Replace(item);
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