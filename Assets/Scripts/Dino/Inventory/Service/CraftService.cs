using System.Collections.Generic;
using System.Linq;
using Dino.Inventory.Config;
using Dino.Inventory.Model;
using JetBrains.Annotations;
using Logger.Extension;
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
        public void Craft(string craftItemId, HashSet<ItemId> ingredients)
        {
            var recipe = _craftConfig.GetRecipe(craftItemId);
            if (!IsPossibleCraft(recipe)) {
                this.Logger().Error($"Error Craft, craft is not possible craftItemId:= {craftItemId}");
                return;
            }
            if (!ingredients.All(ingredient => _inventoryService.Contains(ingredient))) {
                this.Logger().Error($"Error Craft, inventoryService doesn't contain any ingredients:= {Join(", ", ingredients)}");
                return;
            }
            ingredients.ForEach(ingredient => _inventoryService.Remove(ingredient));
            _inventoryService.Add(recipe.CraftItemId);
        }      
        public void Craft(string craftItemId)
        {
            var recipe = _craftConfig.GetRecipe(craftItemId);
            if (!IsPossibleCraft(recipe)) {
                this.Logger().Error($"Error Craft, craft is not possible craftItemId:= {craftItemId}");
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