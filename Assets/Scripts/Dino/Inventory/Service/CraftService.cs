using System.Collections.Generic;
using System.Linq;
using Dino.Inventory.Config;
using Dino.Inventory.Model;
using JetBrains.Annotations;
using Logger.Extension;
using ModestTree;
using SuperMaxim.Core.Extensions;
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

        [CanBeNull]
        public CraftRecipeConfig FindFirstPossibleRecipe(HashSet<ItemId> ingredients)
        {
            var recipes = FindAllPossibleRecipes(ingredients).ToList();
            if (recipes.IsEmpty()) {
                return null;
            }
            if (recipes.Count > 1) {
                this.Logger().Warn($"Count of recipes > 1 by ingredients:= {Join(", ", ingredients)}," 
                                   + $" recipes:= {Join(", ", recipes.Select(it => it.CraftItemId))}");
            }
            return recipes.First();
        }

        public IEnumerable<CraftRecipeConfig> FindAllPossibleRecipes(HashSet<ItemId> ingredients)
        {
            var groupingIngredients = ingredients.GroupBy(p => p.Name).ToDictionary(it => it.Key, it => it.Count());
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

        public void Craft(HashSet<ItemId> ingredients)
        {
            var recipe = FindFirstPossibleRecipe(ingredients);
            if (recipe == null) {
                this.Logger().Error($"Error Craft, recipe not found by ingredients:= {Join(", ", ingredients)}");
                return;
            }
            if (!HasIngredientsInInventory(recipe)) {
                this.Logger().Error($"Error Craft, craft is not possible recipe:= {recipe.CraftItemId}");
                return;
            }
            ingredients.ForEach(ingredient => _inventoryService.Remove(ingredient));
            _inventoryService.Add(recipe.CraftItemId);
        }

        public void Craft(string recipeId)
        {
            var recipe = _craftConfig.GetRecipe(recipeId);
            if (!HasIngredientsInInventory(recipe)) {
                this.Logger().Error($"Error Craft, ingredients don't contain in inventory:= {recipeId}");
                return;
            }
            recipe.Ingredients.ForEach(ingredient => {
                var items = _inventoryService.GetAll(ingredient.Name).ToList();
                items.Skip(items.Count - ingredient.Count).ForEach(it => _inventoryService.Remove(it));
            });
            _inventoryService.Add(recipe.CraftItemId);
        }
    }
}