using System.Linq;
using Dino.Inventory.Message;
using Dino.Inventory.Service;
using Dino.Loot.Messages;
using Dino.Player.Progress.Service;
using Dino.Session.Messages;
using Dino.UI.Screen.World.Inventory;
using Dino.UI.Tutorial;
using SuperMaxim.Messaging;
using UnityEngine;
using Zenject;

namespace Dino.Tutorial
{
    public class UiCraftScenario : TutorialScenario
    {
        private const int FIRST_LEVEL_WHERE_DROP_IS_ENABLED = 4;
        
        private static readonly string[] TutorialRecipes = {"Bow1", "ThrowingAxe1", "ThrowingAxe2"};
        
        [Inject] private IMessenger _messenger;
        [Inject] private CraftService _craftService;
        [Inject] private CraftTutorial _craftTutorial;
        [Inject] private UiInventorySettings _uiInventorySettings;
        [Inject] private PlayerProgressService _playerProgressService;

        public override void Init()
        {
            if (!_uiInventorySettings.IsCraftEnabled || !IsEnabled || IsCompleted) {
                return;
            }

            _messenger.Subscribe<SessionEndMessage>(msg => Dispose());
            _messenger.Subscribe<LootCollectedMessage>(OnLootCollected);
            _messenger.Subscribe<ItemCraftedMessage>(OnItemCrafted);
            _uiInventorySettings.IsDropEnabled = _playerProgressService.Progress.LevelNumber >= FIRST_LEVEL_WHERE_DROP_IS_ENABLED;
        }

        public void Dispose()
        {
            _messenger.Unsubscribe<SessionEndMessage>(msg => Dispose());
            _messenger.Unsubscribe<LootCollectedMessage>(OnLootCollected);
            _messenger.Unsubscribe<ItemCraftedMessage>(OnItemCrafted);
        }

        private void OnLootCollected(LootCollectedMessage _)
        {
            foreach (var recipe in TutorialRecipes)
            {
                if (IsStepCompleted(recipe)) continue;
                if (!_craftService.HasIngredientsForRecipe(recipe)) continue;
                Debug.Log($"show tutorial for recipe {recipe}");                
                _craftTutorial.Play(recipe);
            }
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