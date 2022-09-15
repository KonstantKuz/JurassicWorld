using Dino.UI.Screen.World.Inventory.Model;
using Dino.Util;
using Feofun.Tutorial.UI;
using Feofun.Util.SerializableDictionary;
using JetBrains.Annotations;
using Logger.Extension;
using SuperMaxim.Core.Extensions;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Dino.UI.Screen.World.Inventory.View
{
    [RequireComponent(typeof(TutorialUiElement))]
    public class InventoryItemView : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField]
        private Image _icon;
        [SerializeField]
        private SerializableDictionary<ItemViewState, GameObject> _stateContainers;
        [SerializeField]
        private GameObject _canCraftContainer;
        [SerializeField]
        private WeaponView _weaponView;
        
        
        private CompositeDisposable _disposable;
        
        [CanBeNull]
        public ItemViewModel Model { get; private set; }

        public void Init(ItemViewModel model)
        {
            Dispose();
            _disposable = new CompositeDisposable();
            
            Model = model;
            _weaponView.Init(model.WeaponModel);
            model.State.Subscribe(UpdateState).AddTo(_disposable);
            model.CanCraft.Subscribe(UpdateCraftState).AddTo(_disposable);

            if (model.Icon != null) {
                _icon.sprite = Resources.Load<Sprite>(IconPath.GetInventory(model.Icon));
            }
            if (model.Item != null) {
                GetComponent<TutorialUiElement>().Id = model.Item.Id.FullName;
            }
        }
        
        public void InitViewForDrag(ItemViewModel model)
        {
            _icon.sprite = Resources.Load<Sprite>(IconPath.GetInventory(model.Icon));
            _weaponView.Init(model.WeaponModel);
            UpdateState(model.State.Value);   
            UpdateCraftState(model.CanCraft.Value);
        }

        private void UpdateCraftState(bool canCraft)
        {
            _canCraftContainer.SetActive(canCraft);
        }

        private void UpdateState(ItemViewState state)
        {
            _stateContainers.Values.ForEach(it => it.SetActive(false));
            SetItemsVisibility(state != ItemViewState.Empty);
            
            if (!_stateContainers.ContainsKey(state)) {
                this.Logger().Error($"State container not found for inventory item state:= {state}");
                return;
            }
            _stateContainers[state].SetActive(true);
        }

        private void SetItemsVisibility(bool visible)
        {
            _icon.gameObject.SetActive(visible);
            _weaponView.gameObject.SetActive(visible);
        }

        private void Dispose()
        {
            Model = null;
            _disposable?.Dispose();
            _disposable = null;
        }
        private void OnDestroy()
        {
            Dispose();
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            Model?.OnClick?.Invoke();
        }
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            Model?.OnBeginDrag?.Invoke(Model);
        }

        public void OnDrag(PointerEventData eventData)
        {
         
        }
        public void OnEndDrag(PointerEventData eventData)
        {
            Model?.OnEndDrag?.Invoke(Model);
        }

   
    }
}