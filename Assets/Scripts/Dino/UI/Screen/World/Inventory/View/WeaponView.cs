using Dino.UI.Screen.World.Inventory.Model;
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
        
        public void Init(WeaponViewModel model)
        {
            Dispose();
            DisableView();
            if (!model.Enabled) {
                return;
            }
            _disposable = new CompositeDisposable();
            SetWeaponWrapper(model);
            _reloadingView.Init(model.WeaponWrapper?.Timer);
            UpdateInactiveOverlay();
        }

        private void DisableView()
        {
            _ammoCount.text = "";
            _inactiveWeaponOverlay.SetActive(false);
        }

        private void SetWeaponWrapper(WeaponViewModel model)
        {
            _weaponWrapper = model.WeaponWrapper;
            if (model.AmmoCountEnabled) { 
                _weaponWrapper?.Clip.AmmoCount.Subscribe(UpdateAmmoCount).AddTo(_disposable);
            }
            _weaponWrapper?.Timer.IsAttackReady.Subscribe(_ => UpdateInactiveOverlay()).AddTo(_disposable);
        }

        private void UpdateAmmoCount(int ammoCount)
        {
            _ammoCount.text = ammoCount.ToString();
            UpdateInactiveOverlay();
        }
        private void UpdateInactiveOverlay()
        {
            _inactiveWeaponOverlay.SetActive(_weaponWrapper is {IsWeaponReadyToFire: false});
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