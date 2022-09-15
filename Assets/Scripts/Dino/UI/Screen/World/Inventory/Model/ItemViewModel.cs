using System;
using Dino.Inventory.Model;
using Dino.Weapon.Components;
using JetBrains.Annotations;
using UniRx;

namespace Dino.UI.Screen.World.Inventory.Model
{
    public class ItemViewModel
    {
        private readonly ReactiveProperty<ItemViewState> _state; 
        private readonly BoolReactiveProperty _canCraft;

        [CanBeNull]
        public Item Item { get; }
        
        [CanBeNull] 
        public string Icon { get; }
        
        public WeaponViewModel WeaponModel { get; }
        public IReactiveProperty<ItemViewState> State => _state;
        public IReactiveProperty<bool> CanCraft => _canCraft;
        
        [CanBeNull]
        public Action OnClick { get; }
        [CanBeNull]
        public Action<ItemViewModel> OnBeginDrag { get; }
        [CanBeNull]
        public Action<ItemViewModel> OnEndDrag { get; }

        public ItemViewModel([CanBeNull] Item item,
                             ItemViewState state,
                             WeaponViewModel weaponModel,
                             bool canCraft = false, 
                             Action onClick = null,
                             Action<ItemViewModel> onBeginDrag = null,
                             Action<ItemViewModel> onEndDrag = null)
        {
            Item = item;
            Icon = item == null ? null : Item.Name;

            _state = new ReactiveProperty<ItemViewState>(state);
            _canCraft = new BoolReactiveProperty(canCraft);
            WeaponModel = weaponModel;
            OnClick = onClick;
            OnBeginDrag = onBeginDrag;
            OnEndDrag = onEndDrag;
        }
        public void UpdateState(ItemViewState state)
        {
            _state.SetValueAndForceNotify(state);
        } 
        public void UpdateCraftState(bool canCraft)
        {
            _canCraft.SetValueAndForceNotify(canCraft);
        }

        public static ItemViewModel ForDrag(Item item, bool canCraft)
        {
            return new ItemViewModel(item, ItemViewState.Inactive, WeaponViewModel.CreateDisable(), canCraft);
        }

        public static ItemViewModel Empty()
        {
            return new ItemViewModel(null, ItemViewState.Empty, WeaponViewModel.CreateDisable());
        }
    }
}