using Dino.Weapon.Components;
using JetBrains.Annotations;
using TMPro;
using UniRx;
using UnityEngine;

namespace Dino.UI.Screen.World.Inventory.View
{
    public class WeaponView : MonoBehaviour
    {
        [SerializeField] 
        private TextMeshProUGUI _ammoCount;
        [SerializeField]
        private ItemReloadingView _reloadingView;
        [SerializeField]
        private GameObject _inactiveWeaponOverlay;
        
        private CompositeDisposable _disposable;

        [CanBeNull]
        private WeaponWrapper _weaponWrapper;
        
        public void Init([CanBeNull] WeaponWrapper weaponWrapper)
        {
            Dispose();
            _disposable = new CompositeDisposable();
            _weaponWrapper = weaponWrapper;
            
            _reloadingView.Init(weaponWrapper?.Timer);
            UpdateInactiveOverlay();
            if (weaponWrapper == null) {
                _ammoCount.text = "";
            } else {
                weaponWrapper.Clip.AmmoCount.Subscribe(UpdateAmmoCount).AddTo(_disposable);
                weaponWrapper.Timer.IsAttackReady.Subscribe(_ => UpdateInactiveOverlay()).AddTo(_disposable);
            }
        }
        private void UpdateAmmoCount(int ammoCount)
        {
            _ammoCount.text = ammoCount.ToString();
            UpdateInactiveOverlay();
        }

        private void UpdateInactiveOverlay()
        {
            if (_weaponWrapper == null) {
                _inactiveWeaponOverlay.SetActive(false);
            } else {
                _inactiveWeaponOverlay.SetActive(!_weaponWrapper.Clip.HasAmmo || !_weaponWrapper.Timer.IsAttackReady.Value);
            }
        }

        private void OnDestroy()
        {
            Dispose();
        }

        private void Dispose()
        {
            _disposable?.Dispose();
            _disposable = null;
            _weaponWrapper = null;
        }
    }
}