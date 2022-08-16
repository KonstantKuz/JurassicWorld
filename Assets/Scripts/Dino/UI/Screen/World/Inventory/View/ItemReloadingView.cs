using DG.Tweening;
using Dino.Weapon.Components;
using JetBrains.Annotations;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Dino.UI.Screen.World.Inventory.View
{
    public class ItemReloadingView : MonoBehaviour
    {
        [SerializeField]
        private GameObject _reloadOverlay;
        [SerializeField]
        private Image _reloadBar;

        private CompositeDisposable _disposable;
        
        private WeaponTimer _weaponTimer;
        private Tween _reloadAnimation;

        public void Init([CanBeNull] WeaponTimer weaponTimer)
        {
            Dispose();
            _disposable = new CompositeDisposable();

            if(weaponTimer == null) return;
         
            _weaponTimer = weaponTimer;
            _weaponTimer.IsAttackReady.Subscribe(PlayReloadAnimation).AddTo(_disposable);
        }
        

        private void PlayReloadAnimation(bool isAttackReady)
        {
            _reloadOverlay.SetActive(!isAttackReady);
            
            if(isAttackReady) return;
            
            _reloadAnimation?.Kill();
            _reloadBar.fillAmount = _weaponTimer.ReloadProgress;
            _reloadAnimation = DOTween.To(() => _reloadBar.fillAmount, value => { _reloadBar.fillAmount = value; }, 1, _weaponTimer.ReloadTimeLeft).SetEase(Ease.Linear);
        }

        private void OnDestroy()
        {
            Dispose();
        }

        private void Dispose()
        {
            _reloadAnimation?.Kill();
            _disposable?.Dispose();
            _disposable = null;
        }
    }
}