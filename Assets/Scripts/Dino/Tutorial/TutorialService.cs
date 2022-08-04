using System.Linq;
using Dino.Inventory.Message;
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
        private static readonly string[] TutorialReceipts = {"Bow1", "ThrowingAxe1", "ThrowingAxe2"};
        
        [Inject] private IMessenger _messenger;
        [Inject] private CraftService _craftService;
        [Inject] private TutorialRepository _repository;
        
        public void OnWorldSetup()
        {
            _messenger.Subscribe<LootCollectedMessage>(OnLootCollected);
            _messenger.Subscribe<ItemCraftedMessage>(OnItemCrafted);
        }

        public void OnWorldCleanUp()
        {
            _messenger.Unsubscribe<LootCollectedMessage>(OnLootCollected);
            _messenger.Unsubscribe<ItemCraftedMessage>(OnItemCrafted);
        }

        private void OnLootCollected(LootCollectedMessage _)
        {
            foreach (var receipt in TutorialReceipts)
            {
                if (IsStateCompleted(receipt)) continue;
                if (!_craftService.HasIngredientsForReceipt(receipt)) continue;
                Debug.Log($"show tutorial for recipe {receipt}");
            }
        }

        private void CompleteStep(string stepName)
        {
            var state = GetState();
            state.CompletedStages.Add(stepName);
            _repository.Set(state);
        }

        private TutorialState GetState()
        {
            return _repository.Get() ?? new TutorialState();
        }

        private bool IsStateCompleted(string stepName)
        {
            var state = GetState();
            return state.CompletedStages.Contains(stepName);
        }

        private void OnItemCrafted(ItemCraftedMessage msg)
        {
            if (!TutorialReceipts.Contains(msg.ItemId)) return;
            CompleteStep(msg.ItemId);
            Debug.Log($"complete tutorial for recipe {msg.ItemId}");
        }
    }
}