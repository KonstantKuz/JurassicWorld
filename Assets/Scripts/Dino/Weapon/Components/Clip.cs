using Dino.Inventory.Model;
using Dino.Inventory.Service;
using Logger.Extension;
using UniRx;

namespace Dino.Weapon.Components
{
    public class Clip
    {
        private readonly ReactiveProperty<int> _ammoCount = new ReactiveProperty<int>(0);

        private readonly InventoryService _inventoryService;
        private readonly string _ammoId;

        public IReactiveProperty<int> AmmoCount => _ammoCount;
        
        public bool HasAmmo => AmmoCount.Value > 0;

        public Clip(InventoryService inventoryService, string ammoId)
        {
            _inventoryService = inventoryService; 
            inventoryService.InventoryProperty.Subscribe(UpdateAmmoCount);
            _ammoId = ammoId;
        }

        public void OnFire() => _inventoryService.DecreaseItemAmount(_ammoId, 1);

        private void UpdateAmmoCount(Inventory.Model.Inventory inventory)
        {
            var ammoItem = inventory.FindItem(_ammoId);
            if (ammoItem == null) {
                AmmoCount.Value = 0;
                return;
            }
            if (ammoItem.Type != InventoryItemType.Ammo) {
                this.Logger().Error($"Ammo item is not type ammo, item:= {ammoItem}");
                return;
            }
            _ammoCount.Value = ammoItem.Amount;
        }
    }
}