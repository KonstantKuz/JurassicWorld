using Feofun.ReceivingLoot.Config;
using Feofun.ReceivingLoot.Message;
using Feofun.ReceivingLoot.Model;
using Feofun.UI;
using Feofun.UI.Loader;
using SuperMaxim.Messaging;
using UnityEngine;
using Zenject;

namespace Feofun.ReceivingLoot.Component
{
    public class FlyingIconVfxReceiver : MonoBehaviour
    {
        [SerializeField]
        private string _lootType;
        [SerializeField]
        private RectTransform _receivingContainer;
        [SerializeField]
        private FlyingIconVfxConfig _vfxConfig;

        [Inject] private IMessenger _messenger;
        [Inject] private UILoader _uiLoader;
        [Inject] private UIRoot _uiRoot;

        private FlyingIconVfxPlayer _vfxPlayer;

        private void OnEnable() => _messenger.Subscribe<FlyingIconVfxReceivedMessage>(OnLootReceived);
        public void OnDisable() => _messenger.Unsubscribe<FlyingIconVfxReceivedMessage>(OnLootReceived);

        private void OnLootReceived(FlyingIconVfxReceivedMessage msg)
        {
            if (!msg.Type.Equals(_lootType)) {
                return;
            }
            PlayVfx(msg);
        }

        private void PlayVfx(FlyingIconVfxReceivedMessage msg)
        {
            FlyingIconVfxPlayer.Play(FlyingIconVfxParams.FromReceivedMessage(msg, _receivingContainer.position));
        }

        private FlyingIconVfxPlayer FlyingIconVfxPlayer =>
                _vfxPlayer ??= gameObject.AddComponent<FlyingIconVfxPlayer>().Init(_uiLoader, _vfxConfig, _uiRoot.ReceivedLootContainer);
    }
}