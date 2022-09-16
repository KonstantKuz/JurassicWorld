using Feofun.DroppingLoot.Config;
using Feofun.DroppingLoot.Message;
using Feofun.DroppingLoot.Model;
using Feofun.UI;
using Feofun.UI.Loader;
using SuperMaxim.Messaging;
using UnityEngine;
using Zenject;

namespace Feofun.DroppingLoot.Component
{
    public class DroppingLootReceiver : MonoBehaviour
    {
        [SerializeField]
        private string _lootType;    
        [SerializeField]
        private RectTransform _receivingContainer;    
        [SerializeField]
        private DroppingLootConfig _config;  
      
        [Inject] private IMessenger _messenger;    
        [Inject] private UILoader _uiLoader;         
        [Inject] private UIRoot _uiRoot;

        private DroppingLootVfx _vfx;
        
        private void OnEnable() => _messenger.Subscribe<UILootReceivedMessage>(OnDroppingLootReceived);

        private void OnDroppingLootReceived(UILootReceivedMessage msg)
        {
            if (!msg.Type.Equals(_lootType)) {
                return;
            }
            PlayVfx(msg);
        }

        private void PlayVfx(UILootReceivedMessage msg) => DroppingLootVfx.Play(DroppingLootInitParams.FromReceivedMessage(msg, _receivingContainer.position));

        public void OnDisable() => _messenger.Unsubscribe<UILootReceivedMessage>(OnDroppingLootReceived);

        private DroppingLootVfx DroppingLootVfx => _vfx ??= gameObject.AddComponent<DroppingLootVfx>().Init(_uiLoader, _config, _uiRoot.DroppingLootContainer);
    }
}