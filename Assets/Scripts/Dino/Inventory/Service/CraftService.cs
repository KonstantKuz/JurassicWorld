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
        [Inject] public CraftConfig _craftConfig;
        [Inject] public InventoryService _inventoryService;
        [Inject] private PlayerProgressService _playerProgressService;
        [Inject] private Analytics.Analytics _analytics;
        [Inject] private IMessenger _messenger;

        [CanBeNull]
        public CraftRecipeConfig FindFirstMatchingRecipe(HashSet<Item> ingredients)
        {
            var recipes = FindAllMatchingRecipes(ingredients).ToList();
            if (recipes.IsEmpty()) {
                return null;
            }
            if (recipes.Count > 1) {
                this.Logger()
                    .Warn($"Count of recipes > 1 by ingredients:= {Join(", ", ingredients)},"
                          + $" recipes:= {Join(", ", recipes.Select(it => it.CraftItemId))}");
            }
            return recipes.First();
        }

        [CanBeNull]
        public CraftRecipeConfig FindFirstPossibleRecipe(HashSet<Item> ingredients)
        {
            var recipe = FindFirstMatchingRecipe(ingredients);
            if (recipe == null) {
                return null;
            }
            return !HasIngredientsInInventory(recipe) ? null : recipe;
        }

        public IEnumerable<CraftRecipeConfig> FindAllMatchingRecipes(HashSet<Item> ingredients)
        {
            var ingredientsMap = ingredients.ToDictionary(it => it.Id.FullName, it => it.Amount);
            foreach (var recipe in _craftConfig.Crafts.Values) {
                if (AreIngredientsMatchingRecipe(recipe, ingredientsMap)) {
                    yield return recipe;
                }
            }
        }

        private bool AreIngredientsMatchingRecipe(CraftRecipeConfig recipe, Dictionary<string, int> ingredients)
        {
            if (ingredients.Count != recipe.Ingredients.Count) {
                return false;
            }
            return recipe.Ingredients.All(ingredient => ingredients.ContainsKey(ingredient.Id) && ingredients[ingredient.Id] == ingredient.Count);
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
            return recipe.Ingredients.All(ingredient => _inventoryService.GetAmount(ItemId.Create(ingredient.Id)) >= ingredient.Count);
        }

        public bool HasIngredientsForRecipe(string recipeId)
        {
            var recipe = _craftConfig.FindRecipe(recipeId);
            return recipe != null && HasIngredientsInInventory(recipe);
        }

        public CraftRecipeConfig GetRecipeConfig(string recipeId) => _craftConfig.GetRecipe(recipeId);

        public Item Craft(HashSet<Item> ingredients)
        {
            var recipe = FindFirstPossibleRecipe(ingredients);
            if (recipe == null) {
                throw new ArgumentException($"Error Craft, recipe not found by ingredients := {Join(", ", ingredients)} or ingredients don't contain in inventory");
            }
            ingredients.ForEach(ingredient => _inventoryService.Remove(ingredient.Id));
            var craftedItem = _inventoryService.Add(ItemId.Create(recipe.CraftItem.Id), recipe.CraftItem.Type, recipe.CraftItem.Count);
            ReportCraftedItem(recipe);
            return craftedItem;
        }

        public Item Craft(string recipeId)
        {
            var recipe = _craftConfig.GetRecipe(recipeId);
            if (!HasIngredientsInInventory(recipe)) {
                throw new ArgumentException($"Error crafting, ingredients don't contain in inventory:= {recipeId}");
            }
            recipe.Ingredients.ForEach(ingredient => { _inventoryService.Remove(ItemId.Create(ingredient.Id), ingredient.Count); });
            var craftedItem = _inventoryService.Add(ItemId.Create(recipe.CraftItem.Id), recipe.CraftItem.Type, recipe.CraftItem.Count);
            ReportCraftedItem(recipe);
            return craftedItem;
        }

        private void ReportCraftedItem(CraftRecipeConfig recipe)
        {
            _playerProgressService.Progress.IncreaseCraftCount();
            _analytics.ReportCraftItem(recipe.CraftItemId);
            _messenger.Publish(new ItemCraftedMessage {
                    ItemId = recipe.CraftItemId
            });
        }
    }
}