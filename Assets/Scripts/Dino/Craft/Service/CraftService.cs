using System.Collections.Generic;
using System.Linq;
using Dino.Craft.Config;
using Dino.Inventory.Model;
using Dino.Inventory.Service;
using JetBrains.Annotations;
using Logger.Extension;
using Zenject;

namespace Dino.Craft.Service
{
    public class CraftService
    {
        [Inject]
        public CraftConfig _craftConfig;

        [Inject]
        public InventoryService _inventoryService;

        [CanBeNull]
        public CraftRecipeConfig GetPossibleCraft(ItemId ingredientId)
        {
            var itemName = ingredientId.Name;
            foreach (var recipe in _craftConfig.Crafts.Values) {
                if (!recipe.ContainsIngredient(itemName)) {
                    continue;
                }
                bool canCraft = recipe.Ingredients.All(ingredient => _inventoryService.Count(ingredient.Name) >= ingredient.Count);
                if (!canCraft) {
                    continue;
                }
                return recipe;
            }
            return null;
        }  
        public IEnumerable<CraftRecipeConfig> GetAllPossibleCrafts()
        {
            foreach (var recipe in _craftConfig.Crafts.Values) {
                bool canCraft = recipe.Ingredients.All(ingredient => _inventoryService.Count(ingredient.Name) >= ingredient.Count);
                if (!canCraft) {
                    continue;
                }
                yield return recipe;
            }
        }  
        public bool IsPossibleCraft(CraftRecipeConfig recipe)
        {
            return recipe.Ingredients.All(ingredient => _inventoryService.Count(ingredient.Name) >= ingredient.Count);
        }
        public void Craft(string craftItemId, List<ItemId> ingredients)
        {
            var recipe = _craftConfig.GetRecipe(craftItemId);
            if (IsPossibleCraft(recipe)) {
                this.Logger().Error($"Error Craft, craft is not possible craftItemId:= {craftItemId}");
                return;
            }
            if (ingredients.SequenceEqual(ingredients)) {
                return;
            }
            foreach (var ingredients in recipe.Ingredients) {
                
            }
        }
    }
}