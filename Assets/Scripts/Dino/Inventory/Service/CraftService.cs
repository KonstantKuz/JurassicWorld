﻿using System;
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
            var ingredientsMap = ingredients.ToDictionary(it => it.FullName, it => it.Amount);
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
            return recipe.Ingredients.All(ingredient => ingredients.ContainsKey(ingredient.Name)
                                                        && ingredients[ingredient.Name] == ingredient.Count);
        }
        [CanBeNull]
        public CraftRecipeConfig FindHighestRankPossibleRecipeBy(string craftItemName)
        {
            return GetAllPossibleRecipes()
                   .Select(config => (config, ItemId.SplitFullNameToNameAndRank(config.CraftItemId)))
                   .Where(it => it.Item2.Item1.Equals(craftItemName))
                   .OrderByDescending(it => it.Item2.Item2)
                   .Select(it => it.Item1)
                   .FirstOrDefault();
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

        public bool HasIngredientsForRecipe(string recipeName)
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
            var craftedItem = _inventoryService.Add(recipe.CraftItemId, InventoryItemType.Weapon, 1); // todo replace when changing craft, todo - _inventoryService.Add.(recipe.CraftItemId, recipe.CraftType, recipe.Amount)
            _messenger.Publish(new ItemCraftedMessage {
                    ItemId = recipe.CraftItemId
            });
            return craftedItem;
        }

        public ItemId Craft(string recipeId)
        {
            var recipe = _craftConfig.GetRecipe(recipeId);
            if (!HasIngredientsInInventory(recipe)) {
                throw new ArgumentException($"Error crafting, ingredients don't contain in inventory:= {recipeId}");
            }
            recipe.Ingredients.ForEach(ingredient => {
                _inventoryService.DecreaseItems(ingredient.Name, ingredient.Count);
            });
            var craftedItem = _inventoryService.Add(recipe.CraftItemId, InventoryItemType.Weapon, 1); // todo replace when changing craft, todo - _inventoryService.Add.(recipe.CraftItemId, recipe.CraftType, recipe.Amount)
            _messenger.Publish(new ItemCraftedMessage {
                    ItemId = recipe.CraftItemId
            });
            _analytics.ReportCraftItem(recipe.CraftItemId);
            return craftedItem;
        }

        public CraftRecipeConfig GetRecipeConfig(string recipe)
        {
            return _craftConfig.GetRecipe(recipe);
        }
    }
}