using System;
using System.Linq;
using Dino.Extension;
using Dino.Inventory.Extension;
using Dino.Inventory.Model;
using Dino.Inventory.Service;
using Dino.Weapon.Config;
using Feofun.Config;
using Feofun.ReceivingLoot;
using JetBrains.Annotations;
using Logger.Extension;
using SuperMaxim.Messaging;
using UniRx;
using UnityEngine;

namespace Dino.Location.Workbench
{
    public class CrafterByTimer : IDisposable
    {
        private readonly ReactiveProperty<bool> _hasActiveTimer = new ReactiveProperty<bool>(false);
        private readonly CraftService _craftService;
        private readonly InventoryService _inventoryService;
        private readonly StringKeyedConfigCollection<WeaponConfig> _weaponConfigs;
        private readonly FlyingIconReceivingManager _flyingIconManager;
        
        private readonly string _craftItemId;
        private readonly float _craftDuration;
        private readonly Vector3 _craftItemPosition;
        
        
        private CompositeDisposable _disposable;
        [CanBeNull]
        private ActionTimer _craftTimer;

        public ActionTimer CraftTimer => _craftTimer;
        
        public IReactiveProperty<bool> HasActiveTimer => _hasActiveTimer;

        public CrafterByTimer(InventoryService inventoryService, 
                              CraftService craftService,
                              FlyingIconReceivingManager flyingIconManager,
                              StringKeyedConfigCollection<WeaponConfig> weaponConfigs,
                              string craftItemId,
                              float craftDuration,
                              Vector3 craftItemPosition)
        {
            Dispose();
            _disposable = new CompositeDisposable();
            _craftService = craftService;
            _inventoryService = inventoryService;
            _flyingIconManager = flyingIconManager;
            _weaponConfigs = weaponConfigs;
            _craftItemId = craftItemId;
            _craftDuration = craftDuration;
            _craftItemPosition = craftItemPosition;
            inventoryService.InventoryProperty.Subscribe(it => OnInventoryUpdated()).AddTo(_disposable);
        }

        public void Update() => _craftTimer?.IncreaseProgress();

        private void OnInventoryUpdated()
        {
            if (_craftTimer == null && CanCraft()) {
                CreateTimer();
            }
            if (_craftTimer != null && !CanCraft()) {
                DeleteTimer();
            }
        }

        private void CreateTimer()
        {
            DeleteTimer();
            _craftTimer = new ActionTimer(_craftDuration, OnCraft);
            _hasActiveTimer.SetValueAndForceNotify(true);
        }

        private void DeleteTimer()
        {
            _craftTimer?.Dispose();
            _craftTimer = null;
            _hasActiveTimer.SetValueAndForceNotify(false);
        }

        private bool CanCraft() => _craftService.HasIngredientsForRecipe(_craftItemId) && HasWeaponToCraftAmmo();

        private bool HasWeaponToCraftAmmo()
        {
            var recipe = _craftService.GetRecipeConfig(_craftItemId);
            if (recipe.CraftItem.Type != InventoryItemType.Ammo) {
                return true;
            }
            var weapons = _weaponConfigs.Values.Where(it => it.AmmoId == recipe.CraftItemId);
            return weapons.Any(it => _inventoryService.Contains(new ItemId(it.Id)));
        }

        private void OnCraft()
        {
            if (!CanCraft()) {
                this.Logger().Warn($"Recipe crafting error, missing ingredients, craftItemId:= {_craftItemId}");
                return;
            }
            DeleteTimer();
            var recipeConfig = _craftService.GetRecipeConfig(_craftItemId);
            var item = _craftService.Craft(recipeConfig.CraftItemId);
            _flyingIconManager.ReceiveIcons(item.ToFlyingIconReceivingParams(recipeConfig.CraftItem.Count, _craftItemPosition.WorldToScreenPoint()));
        }

        public void Dispose()
        {
            _disposable?.Dispose();
            _disposable = null;
            DeleteTimer();
        }
    }
}