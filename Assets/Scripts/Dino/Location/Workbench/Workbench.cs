using System;
using System.Linq;
using Dino.Inventory.Config;
using Dino.Inventory.Model;
using Dino.Inventory.Service;
using Dino.Units;
using Dino.Units.Service;
using JetBrains.Annotations;
using Logger.Extension;
using UnityEngine;
using Zenject;
using Unit = Dino.Units.Unit;

namespace Dino.Location.Workbench
{
    public class Workbench : MonoBehaviour
    {
        [SerializeField]
        private string _craftItemName;

        [Inject]
        private CraftService _craftService;
        [Inject]
        private ActiveItemService _activeItemService;
        public event Action OnPlayerTriggered;
        public bool IsPlayerInCraftingArea { get; private set; }

        public string CraftItemName => _craftItemName;

        public bool CanCraftRecipe()
        {
            return FindHighestRankPossibleRecipeBy(CraftItemName) != null;
        }
        public void Craft()
        {
            var recipe = FindHighestRankPossibleRecipeBy(CraftItemName);
            if (recipe == null) {
                this.Logger().Error($"Workbench, recipe crafting error, missing ingredients, craftItemName:= {_craftItemName}");
                return;
            }
            var item = _craftService.Craft(recipe.CraftItemId);
            _activeItemService.Replace(item);
        }

        private void OnTriggerEnter(Collider collider)
        {
            OnPlayerTrigger(true);
        }

        private void OnTriggerExit(Collider collider)
        {   
            OnPlayerTrigger(false);
        }

        [CanBeNull]
        private CraftRecipeConfig FindHighestRankPossibleRecipeBy(string craftItemName)
        {
            return _craftService.GetAllPossibleRecipes()
                                .Select(config => (config, ItemId.SplitFullNameToNameAndRank(config.CraftItemId)))
                                .Where(it => it.Item2.Item1.Equals(craftItemName))
                                .OrderByDescending(it => it.Item2.Item2)
                                .Select(it => it.Item1)
                                .FirstOrDefault();
        }
        private void OnPlayerTrigger(bool entered)
        {
            IsPlayerInCraftingArea = entered;
            OnPlayerTriggered?.Invoke();
        }

        private bool IsPlayer(Collider collider)
        {
            var unit = collider.GetComponent<Unit>();
            return unit != null && unit.UnitType == UnitType.PLAYER;
        }
    }
}