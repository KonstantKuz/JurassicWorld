using Dino.Inventory.Config;
using Dino.Inventory.Model;
using Dino.Location;
using Dino.Session.Messages;
using Dino.Session.Service;
using SuperMaxim.Core.Extensions;
using SuperMaxim.Messaging;
using Zenject;

namespace Dino.Inventory.Service
{
    public class ItemProviderService : IWorldScope
    {
        [Inject] private IMessenger _messenger;
        [Inject] private SessionService _sessionService;
        [Inject] private InventoryService _inventoryService;
        [Inject] private ItemProviderConfig _providerConfig;
        
        public void OnWorldSetup()
        {
            _messenger.Subscribe<SessionStartMessage>(OnSessionStarted);
        }

        private void OnSessionStarted(SessionStartMessage msg)
        {
            var providedItems = _providerConfig.FindProvidedItems(msg.LevelId);
            providedItems?.ForEach(it => _inventoryService.Add(ItemId.Create(it.ItemId), it.Type, it.Amount));
        }
        
        public void OnWorldCleanUp()
        {
            _messenger.Unsubscribe<SessionStartMessage>(OnSessionStarted);
        }
    }
}