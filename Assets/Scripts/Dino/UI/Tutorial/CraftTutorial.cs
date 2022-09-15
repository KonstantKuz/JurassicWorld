using System.Linq;
using DG.Tweening;
using Dino.Inventory.Config;
using Dino.Inventory.Service;
using Dino.Location;
using Dino.UI.Screen.World.Inventory.View;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace Dino.UI.Tutorial
{
    [RequireComponent(typeof(InventoryView))]
    public class CraftTutorial : MonoBehaviour
    {
        private const float DRAG_ANIMATION_TIME = 1.0f; 
        
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
            var itemViewFrom = GetFirstIngredientView(receiptConfig);
            Assert.IsNotNull(itemViewFrom, $"Failed to find view for first ingredient in recipe {receiptConfig.CraftItemId}");
            var itemViewTo = GetSecondIngredientView(receiptConfig);
            Assert.IsNotNull(itemViewTo, $"Failed to find view for second ingredient in recipe {receiptConfig.CraftItemId}");
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

        [CanBeNull] 
        private InventoryItemView GetFirstIngredientView(CraftRecipeConfig receiptConfig)
        {
            var firstIngredient = receiptConfig.Ingredients.First().Id;
            return _inventoryView.GetItemViews(firstIngredient).FirstOrDefault();
        }

        [CanBeNull] 
        private InventoryItemView GetSecondIngredientView(CraftRecipeConfig receiptConfig)
        {
            if (receiptConfig.Ingredients.Count == 1)
            {
                var firstIngredient = receiptConfig.Ingredients.First().Id;
                return _inventoryView.GetItemViews(firstIngredient).Skip(1).FirstOrDefault();    
            }

            var secondIngredient = receiptConfig.Ingredients.Skip(1).First().Id;
            return _inventoryView.GetItemViews(secondIngredient).FirstOrDefault();
        }
    }
}