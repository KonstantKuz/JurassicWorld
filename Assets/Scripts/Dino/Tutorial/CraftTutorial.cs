using System;
using System.Linq;
using Dino.Inventory.Service;
using Dino.Location;
using Dino.UI.Screen.World.Inventory.View;
using Feofun.Tutorial.UI;
using UnityEngine;
using Zenject;

namespace Dino.Tutorial
{
    [RequireComponent(typeof(InventoryView))]
    public class CraftTutorial : MonoBehaviour
    {
        [Inject] private CraftService _craftService;
        [Inject] private World _world;
        [Inject] private TutorialUiTools _tutorialUiTools;

        private InventoryView _inventoryView;

        private void Awake()
        {
            _inventoryView = GetComponent<InventoryView>();
        }

        public void Play(string recipe)
        {
            _world.Pause();
            var receiptConfig = _craftService.GetRecipeConfig(recipe);
            var itemView = _inventoryView.GetItemView(receiptConfig.Ingredients.First());
            _tutorialUiTools.ElementHighlighter.Set(itemView);
        }

        public void Stop()
        {
            _world.UnPause();
            _tutorialUiTools.ElementHighlighter.Clear();
        }
    }
}