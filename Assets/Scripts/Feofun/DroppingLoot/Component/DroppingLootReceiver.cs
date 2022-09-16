using Codice.CM.Client.Differences;
using Dino.Inventory.Model;
using Dino.Location;
using EasyButtons;
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
        private DroppingLootType _lootType;    
        [SerializeField]
        private RectTransform _receivingContainer;    
        [SerializeField]
        private DroppingLootConfig _config;  
      
        [Inject] private IMessenger _messenger;    
        [Inject] private UILoader _uiLoader;         
        [Inject] private UIRoot _uiRoot;
        [Inject] private World _world;       
        
        private DroppingLootVfx _vfx;

        [Button]
        public void Test(string lootId)
        {
            var droppingLootType = DroppingLootTypeExt.ValueOf(InventoryItemType.Weapon.ToString());
            var message = UiLootReceivedMessage.Create(droppingLootType, lootId, 1, GetLootScreenPosition());
            _messenger.Publish(message);
        }

        private Vector2 GetLootScreenPosition()
        {
            return Camera.main.WorldToScreenPoint(_world.RequirePlayer().transform.position);
        }

        private void OnEnable()
        {
            _messenger.Subscribe<UiLootReceivedMessage>(OnDroppingLootReceived);
        }
        private void OnDroppingLootReceived(UiLootReceivedMessage msg)
        {
            if (msg.Type != _lootType) {
                return;
            }
            PlayVfx(msg);
        }

        private void PlayVfx(UiLootReceivedMessage msg)
        {
            DroppingLootVfx.Play(DroppingLootModel.FromReceivedMessage(msg, _receivingContainer.position));
        }
        public void OnDisable()
        {
            _messenger.Unsubscribe<UiLootReceivedMessage>(OnDroppingLootReceived);
        }

        private DroppingLootVfx DroppingLootVfx => _vfx ??= gameObject.AddComponent<DroppingLootVfx>().Init(_uiLoader, _config, _uiRoot.DroppingLootContainer);
    }
}