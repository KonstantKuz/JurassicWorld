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
    public class LootVfxReceiver : MonoBehaviour
    {
        [SerializeField]
        private string _lootType;
        [SerializeField]
        private RectTransform _receivingContainer;
        [SerializeField]
        private ReceivedLootVfxConfig _vfxConfig;

        [Inject] private IMessenger _messenger;
        [Inject] private UILoader _uiLoader;
        [Inject] private UIRoot _uiRoot;

        private ReceivedLootVfxPlayer _vfxPlayer;

        private void OnEnable() => _messenger.Subscribe<UILootReceivedMessage>(OnLootReceived);
        public void OnDisable() => _messenger.Unsubscribe<UILootReceivedMessage>(OnLootReceived);

        private void OnLootReceived(UILootReceivedMessage msg)
        {
            if (!msg.Type.Equals(_lootType)) {
                return;
            }
            PlayVfx(msg);
        }

        private void PlayVfx(UILootReceivedMessage msg)
        {
            ReceivedLootVfxPlayer.Play(ReceivedLootVfxParams.FromReceivedMessage(msg, _receivingContainer.position));
        }

        private ReceivedLootVfxPlayer ReceivedLootVfxPlayer =>
                _vfxPlayer ??= gameObject.AddComponent<ReceivedLootVfxPlayer>().Init(_uiLoader, _vfxConfig, _uiRoot.ReceivedLootContainer);
    }
}