using Dino.UI.Screen.World.Inventory.Model;
using Dino.Util;
using Feofun.UI.Components.Button;
using Feofun.Util.SerializableDictionary;
using Logger.Extension;
using SuperMaxim.Core.Extensions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Dino.UI.Screen.World.Inventory.View
{
    public class InventoryItemView : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IPointerUpHandler, IEndDragHandler
    {
        [SerializeField]
        private Image _icon;

        [SerializeField]
        private ActionButton _button;
        [SerializeField]
        private SerializableDictionary<ItemViewState, GameObject> _stateContainers;

        private ItemViewModel _model;
        public ItemViewModel Model => _model;
        
        
        public void Init(ItemViewModel model)
        {
            _model = model;
            if (model.Id == null) {
                return;
            }
            _icon.sprite = Resources.Load<Sprite>(IconPath.GetInventory(model.Id.Name));
            _button.Init(model.OnClick);
            UpdateState(model.State);
        } 
        public void InitDragView(ItemViewModel model)
        {
            _icon.sprite = Resources.Load<Sprite>(IconPath.GetInventory(model.Id.Name));
            UpdateState(ItemViewState.Inactive);
        }

        private void UpdateState(ItemViewState state)
        {
            _stateContainers.Values.ForEach(it => it.SetActive(false));
            if (!_stateContainers.ContainsKey(state)) {
                this.Logger().Error($"State container not found for inventory item state:= {state}");
                return;
            }
            _stateContainers[state].SetActive(true);
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            _model.OnClick?.Invoke();
        }
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            _model.OnBeginDrag?.Invoke(_model);
        }

        public void OnDrag(PointerEventData eventData)
        {
         
        }
        
        public void OnPointerUp(PointerEventData eventData)
        {
            
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _model.OnEndDrag?.Invoke(_model);
        }
    }
}