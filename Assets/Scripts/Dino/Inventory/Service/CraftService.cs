using System;
using System.Collections.Generic;
using System.Linq;
using Dino.Inventory.Config;
using Dino.Inventory.Message;
using Dino.Inventory.Model;
using Dino.Player.Progress.Service;
using JetBrains.Annotations;
using Logger.Extension;
using ModestTree;
using SuperMaxim.Core.Extensions;
using SuperMaxim.Messaging;
using Zenject;
using static System.String;

namespace Dino.Inventory.Service
{
    public class CraftService
    {
        [Inject]
        public CraftConfig _craftConfig;

        [Inject]
        public InventoryService _inventoryService;

        [Inject] private PlayerProgressService _playerProgressService;
        [Inject] private Analytics.Analytics _analytics;
        [Inject] private IMessenger _messenger;

        [CanBeNull]
        public CraftRecipeConfig FindFirstMatchingRecipe(HashSet<ItemId> ingredients)
        {
            var recipes = FindAllMatchingRecipes(ingredients).ToList();
            if (recipes.IsEmpty()) {
                return null;
            }
            if (recipes.Count > 1) {
                this.Logger().Warn($"Count of recipes > 1 by ingredients:= {Join(", ", ingredients)}," 
                                   + $" recipes:= {Join(", ", recipes.Select(it => it.CraftItemId))}");
            }
            return recipes.First();
        }       
        [CanBeNull]
        public CraftRecipeConfig FindFirstPossibleRecipe(HashSet<ItemId> ingredients)
        {
            var recipe = FindFirstMatchingRecipe(ingredients);
            if (recipe == null) {
                return null;
            }
            return !HasIngredientsInInventory(recipe) ? null : recipe;
        }

        public IEnumerable<CraftRecipeConfig> FindAllMatchingRecipes(HashSet<ItemId> ingredients)
        {
            var groupingIngredients = ingredients.GroupBy(p => p.FullName).ToDictionary(it => it.Key, it => it.Count());
            foreach (var recipe in _craftConfig.Crafts.Values) {
                if (AreIngredientsMatchingRecipe(recipe, groupingIngredients)) {
                    yield return recipe;
                }
            }
        }

        private bool AreIngredientsMatchingRecipe(CraftRecipeConfig recipe, Dictionary<string, int> groupingIngredients)
        {
            if (groupingIngredients.Count != recipe.Ingredients.Count) {
                return false;
            }
            return recipe.Ingredients.All(ingredient => groupingIngredients.ContainsKey(ingredient.Name)
                                                        && groupingIngredients[ingredient.Name] == ingredient.Count);
        }

        public IEnumerable<CraftRecipeConfig> GetAllPossibleRecipes()
        {
            foreach (var recipe in _craftConfig.Crafts.Values) {
                if (!HasIngredientsInInventory(recipe)) {
                    continue;
                }
                yield return recipe;
            }
        }

        private bool HasIngredientsInInventory(CraftRecipeConfig recipe)
        {
            return recipe.Ingredients.All(ingredient => _inventoryService.Count(ingredient.Name) >= ingredient.Count);
        }

        public bool HasIngredientsForReceipt(string recipeName)
        {
            var recipe = _craftConfig.GetRecipe(recipeName);
            if (recipe == null) return false;
            return HasIngredientsInInventory(recipe);
        }

        public ItemId Craft(HashSet<ItemId> ingredients)
        {
            var recipe = FindFirstPossibleRecipe(ingredients);
            if (recipe == null) {
                throw new ArgumentException($"Error Craft, recipe not found by ingredients := {Join(", ", ingredients)} or ingredients don't contain in inventory");
            }
            ingredients.ForEach(ingredient => _inventoryService.Remove(ingredient)); 
            _playerProgressService.Progress.IncreaseCraftCount();
            _analytics.ReportCraftItem(recipe.CraftItemId);
            _messenger.Publish(new ItemCraftedMessage
            {
                ItemId = recipe.CraftItemId
            });
            return _inventoryService.Add(recipe.CraftItemId);
        }

        public ItemId Craft(string recipeId)
        {
            var recipe = _craftConfig.GetRecipe(recipeId);
            if (!HasIngredientsInInventory(recipe)) {
                throw new ArgumentException($"Error Craft, ingredients don't contain in inventory:= {recipeId}");
            }
            recipe.Ingredients.ForEach(ingredient => {
                var items = _inventoryService.GetAll(ingredient.Name).ToList();
                items.Skip(items.Count - ingredient.Count).ForEach(it => _inventoryService.Remove(it));
            });
            return _inventoryService.Add(recipe.CraftItemId);
        }

        public CraftRecipeConfig GetRecipeConfig(string recipe)
        {
            return _craftConfig.GetRecipe(recipe);
        }
    }
}