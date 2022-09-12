using System.Linq;
using Dino.Inventory.Model;
using Dino.Inventory.Service;
using Dino.Units.Service;
using Dino.Weapon.Components;
using JetBrains.Annotations;
using UniRx;

namespace Dino.Weapon.Service
{
    public class AutomaticWeaponChangeService
    {
        private readonly WeaponService _weaponService;
        private readonly ActiveItemService _activeItemService;
        private readonly InventoryService _inventoryService;

        private CompositeDisposable _disposable;

        public AutomaticWeaponChangeService(WeaponService weaponService, ActiveItemService activeItemService, InventoryService inventoryService)
        {

            _weaponService = weaponService;
            _activeItemService = activeItemService;
            _inventoryService = inventoryService;
            weaponService.ActiveWeapon.Subscribe(OnUpdateActiveWeapon);
        }

        private void OnUpdateActiveWeapon([CanBeNull] WeaponWrapper weapon)
        {
            Dispose();
            if (weapon == null) {
                return;
            }
            _disposable = new CompositeDisposable();
            weapon.Clip.AmmoCount.Subscribe(OnUpdateAmmoCount).AddTo(_disposable);
        }
        private void OnUpdateAmmoCount(int ammoCount)
        {
            if (ammoCount <= 0) {
                TryChangeWeapon();
            }
        }
        private void TryChangeWeapon()
        {
            var newWeapon = _inventoryService.GetItems(InventoryItemType.Weapon)
                                             .Select(it => _weaponService.GetWeaponWrapper(it.Id))
                                             .FirstOrDefault(it => it.Clip.HasAmmo);
            if (newWeapon == null) {
                return;
            }
            Dispose();
            _activeItemService.Replace(_inventoryService.GetItem(newWeapon.WeaponId));
        }
        private void Dispose()
        {
            _disposable?.Dispose();
            _disposable = null;
        }
    }
}