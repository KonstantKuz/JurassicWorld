using Feofun.ReceivingLoot.Config;
using Feofun.ReceivingLoot.Model;
using Feofun.UI;
using Feofun.UI.Loader;
using UnityEngine;
using Zenject;

namespace Feofun.ReceivingLoot.Component
{
    public class FlyingIconVfxReceiver : MonoBehaviour
    {
        [SerializeField]
        private string _vfxType;
        [SerializeField]
        private RectTransform _receivingContainer;
        [SerializeField]
        private FlyingIconVfxConfig _vfxConfig;

        [Inject] private FlyingIconReceivingManager _flyingIconReceivingManager;
        [Inject] private UILoader _uiLoader;
        [Inject] private UIRoot _uiRoot;

        private FlyingIconVfxPlayer _vfxPlayer;
        
        private FlyingIconVfxPlayer FlyingIconVfxPlayer =>
                _vfxPlayer ??= gameObject.AddComponent<FlyingIconVfxPlayer>().Init(_uiLoader, _vfxConfig, _uiRoot.FlyingIconContainer);
        
        public string VfxType => _vfxType;

        private void OnEnable() => _flyingIconReceivingManager.RegisterReceiver(this);
        public void OnDisable() => _flyingIconReceivingManager.UnregisterReceiver(this);
        
        public void PlayVfx(FlyingIconReceivingParams msg)
        {
            FlyingIconVfxPlayer.Play(FlyingIconVfxParams.FromReceivedMessage(msg, _receivingContainer.position));
        }

  
    }
}