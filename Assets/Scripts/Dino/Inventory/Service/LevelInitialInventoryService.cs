using Dino.Inventory.Config;
using Dino.Inventory.Model;
using Dino.Location;
using Dino.Session.Messages;
using SuperMaxim.Core.Extensions;
using SuperMaxim.Messaging;
using Zenject;

namespace Dino.Inventory.Service
{
    public class LevelInitialInventoryService : IWorldScope
    {
        [Inject] private IMessenger _messenger;
        [Inject] private InventoryService _inventoryService;
        [Inject] private InitialInventoryConfig _initialInventoryConfig;
        
        public void OnWorldSetup()
        {
            _messenger.Subscribe<SessionStartMessage>(OnSessionStarted);
        }

        private void OnSessionStarted(SessionStartMessage msg)
        {
            var providedItems = _initialInventoryConfig.FindProvidedItems(msg.LevelId);
            providedItems?.ForEach(it => _inventoryService.Add(ItemId.Create(it.ItemId), it.Type, it.Amount));
        }
        
        public void OnWorldCleanUp()
        {
            _messenger.Unsubscribe<SessionStartMessage>(OnSessionStarted);
        }
    }
}