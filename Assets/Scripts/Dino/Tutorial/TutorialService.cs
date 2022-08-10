using System.Linq;
using Dino.Inventory.Message;
using Dino.Inventory.Service;
using Dino.Location;
using Dino.Loot.Messages;
using Dino.UI.Tutorial;
using SuperMaxim.Messaging;
using UnityEngine;
using Zenject;

namespace Dino.Tutorial
{
    public class TutorialService: IWorldScope
    {
        private static readonly string[] TutorialRecipes = {"Bow1", "ThrowingAxe1", "ThrowingAxe2"};
        
        [Inject] private IMessenger _messenger;
        [Inject] private CraftService _craftService;
        [Inject] private TutorialRepository _repository;
        [Inject] private CraftTutorial _craftTutorial;

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
            foreach (var recipe in TutorialRecipes)
            {
                if (IsStateCompleted(recipe)) continue;
                if (!_craftService.HasIngredientsForReceipt(recipe)) continue;
                Debug.Log($"show tutorial for recipe {recipe}");                
                _craftTutorial.Play(recipe);
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
            if (!TutorialRecipes.Contains(msg.ItemId)) return;
            CompleteStep(msg.ItemId);
            _craftTutorial.Stop();
            Debug.Log($"complete tutorial for recipe {msg.ItemId}");            
        }
    }
}