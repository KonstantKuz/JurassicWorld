using System.Linq;
using DG.Tweening;
using Dino.Inventory.Config;
using Dino.Inventory.Model;
using Dino.Inventory.Service;
using Dino.Location;
using Dino.UI.Screen.World.Inventory.View;
using UnityEngine;
using Zenject;

namespace Dino.Tutorial
{
    [RequireComponent(typeof(InventoryView))]
    public class CraftTutorial : MonoBehaviour
    {
        private const float DRAG_ANIMATION_TIME = 2.0f; 
        
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
            var itemViewFrom = GetFirstItem(receiptConfig);
            var itemViewTo = GetSecondItem(receiptConfig);
            _tutorialUiTools.ElementHighlighter.Set(new [] {itemViewFrom, itemViewTo});
            var tween = _tutorialUiTools.TutorialHand.ShowDragUI(
                itemViewFrom.transform as RectTransform, 
                itemViewTo.transform as RectTransform,
                DRAG_ANIMATION_TIME);
            tween.SetLoops(-1);
        }
        
        public void Stop()
        {
            _world.UnPause();
            _tutorialUiTools.ElementHighlighter.Clear();
            _tutorialUiTools.TutorialHand.Hide();
        }        

        private InventoryItemView GetFirstItem(CraftRecipeConfig receiptConfig)
        {
            return _inventoryView.GetItemView(receiptConfig.Ingredients.First().Name, 0);
        }

        private InventoryItemView GetSecondItem(CraftRecipeConfig receiptConfig)
        {
            if (receiptConfig.Ingredients.Count == 1)
            {
                return _inventoryView.GetItemView(receiptConfig.Ingredients.First().Name, 1);    
            }

            return _inventoryView.GetItemView(receiptConfig.Ingredients.Skip(1).First().Name);
        }
    }
}