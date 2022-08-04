using System;
using Dino.Inventory.Model;
using JetBrains.Annotations;
using UniRx;

namespace Dino.UI.Screen.World.Inventory.Model
{
    public class ItemViewModel
    {
        private readonly ReactiveProperty<ItemViewState> _state; 
        private readonly BoolReactiveProperty _canCraft;

        [CanBeNull]
        public ItemId Id { get; }
        [CanBeNull]
        public string Icon { get; }
        public int Rank { get; }
        public IReactiveProperty<ItemViewState> State => _state;
        public IReactiveProperty<bool> CanCraft => _canCraft;
        public event Action<float> OnWeaponFireCallback; 
        [CanBeNull]
        public Action OnClick { get; }
        [CanBeNull]
        public Action<ItemViewModel> OnBeginDrag { get; }
        [CanBeNull]
        public Action<ItemViewModel> OnEndDrag { get; }

        public ItemViewModel([CanBeNull] ItemId id,
                             ItemViewState state,
                             bool canCraft = false, 
                             Action onClick = null,
                             Action<ItemViewModel> onBeginDrag = null,
                             Action<ItemViewModel> onEndDrag = null)
        {
            Id = id;
            if (id == null)
            {
                Icon = null;
                Rank = 0;
            }
            else
            {
                Icon = Id.Name;
                Rank = id.Rank;
            }
            _state = new ReactiveProperty<ItemViewState>(state);
            _canCraft = new BoolReactiveProperty(canCraft);
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

        public static ItemViewModel ForDrag(ItemId id, bool canCraft)
        {
            return new ItemViewModel(id, ItemViewState.Inactive, canCraft);
        }

        public static ItemViewModel Empty()
        {
            return new ItemViewModel(null, ItemViewState.Empty);
        }

        public void OnWeaponFire(float attackInterval)
        {
            OnWeaponFireCallback?.Invoke(attackInterval);
        }
    }
}