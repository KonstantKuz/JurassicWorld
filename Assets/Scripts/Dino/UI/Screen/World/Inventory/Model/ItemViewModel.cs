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
        private readonly BoolReactiveProperty _isAttackReady;

        [CanBeNull]
        public ItemId Id { get; }
        [CanBeNull]
        public string Icon { get; }
        public int Rank { get; }
        public IReactiveProperty<ItemViewState> State => _state;
        public IReactiveProperty<bool> CanCraft => _canCraft;
        public BoolReactiveProperty IsAttackReady => _isAttackReady;
        public float ReloadProgress => WeaponTimer.ReloadingTime / WeaponTimer.AttackInterval;
        public float ReloadTimeLeft => WeaponTimer.AttackInterval - WeaponTimer.ReloadingTime;
        
        [CanBeNull]
        public Action OnClick { get; }
        [CanBeNull]
        public Action<ItemViewModel> OnBeginDrag { get; }
        [CanBeNull]
        public Action<ItemViewModel> OnEndDrag { get; }

        [CanBeNull] public WeaponTimer WeaponTimer { get; }

        public ItemViewModel([CanBeNull] ItemId id,
                             ItemViewState state,
                             bool canCraft = false, 
                             WeaponTimer weaponTimer = null,
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

            WeaponTimer = weaponTimer;
            _state = new ReactiveProperty<ItemViewState>(state);
            _canCraft = new BoolReactiveProperty(canCraft);
            _isAttackReady = WeaponTimer?.IsAttackReadyReactiveProperty;
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
    }
}