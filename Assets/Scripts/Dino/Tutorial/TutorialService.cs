using Dino.Inventory.Service;
using Dino.Location;
using Dino.Loot.Messages;
using SuperMaxim.Messaging;
using UnityEngine;
using Zenject;

namespace Dino.Tutorial
{
    public class TutorialService: IWorldScope
    {
        private static string[] _tutorialReceipts = {"Bow1", "ThrowingAxe1", "ThrowingAxe2"};
        
        [Inject] private IMessenger _messenger;
        [Inject] private CraftService _craftService;
        
        public void OnWorldSetup()
        {
            _messenger.Subscribe<LootCollectedMessage>(OnLootCollected);
        }

        public void OnWorldCleanUp()
        {
            _messenger.Unsubscribe<LootCollectedMessage>(OnLootCollected);
        }

        private void OnLootCollected(LootCollectedMessage _)
        {
            foreach (var receipt in _tutorialReceipts)
            {
                if (!_craftService.HasIngredientsForReceipt(receipt)) continue;
                Debug.Log($"show tutorial for recipe {receipt}");
            }
        }
    }
}