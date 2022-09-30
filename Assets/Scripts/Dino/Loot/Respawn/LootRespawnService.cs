using System;
using System.Collections.Generic;
using Dino.Inventory.Config;
using Dino.Inventory.Model;
using Dino.Inventory.Service;
using Dino.Location;
using Dino.Loot.Service;
using Logger.Extension;

namespace Dino.Loot.Respawn
{
    public class LootRespawnService : IWorldScope
    {
        
        private readonly LootFactory _lootFactory;
        private readonly CraftConfig _craftConfig;

        private readonly Dictionary<string, RespawnLootInfo> _respawnLoots = new Dictionary<string, RespawnLootInfo>();

        public LootRespawnService(LootFactory lootFactory, InventoryService inventoryService, CraftConfig craftConfig)
        {
            _lootFactory = lootFactory;
            _craftConfig = craftConfig;
            inventoryService.OnItemChanged += OnItemChanged;
        }
        public void AddLootToRespawn(Loot loot)
        {
            if (loot.ReceivedItem.Type != InventoryItemType.Material) {
                this.Logger().Warn($"Loot type is not Material, loot will not be respawned. Loot:= {loot.ObjectId}, Type:= {loot.ReceivedItem.Type}");
                return;
            }
            var receivedItemId = loot.ReceivedItem.Id;
            var respawnLoot = new RespawnLoot(loot);
            if (!_respawnLoots.ContainsKey(receivedItemId)) {
                _respawnLoots[receivedItemId] = new RespawnLootInfo();
            }
            _respawnLoots[receivedItemId].LootQueue.Enqueue(respawnLoot);
            
        }
        private void OnItemChanged(ItemChangedEvent evn)
        {
            switch (evn.Type) {
                case InventoryItemType.Material:
                    TryIncreaseRespawnAmount(evn); // we need to know how many materials were spent on the current location, because the materials may remain with the last session or be partially crafted
                    return;
                case InventoryItemType.Ammo:
                    TryRespawnLoot(evn);
                    return;
            }
        }

        private void TryRespawnLoot(ItemChangedEvent evn)
        {
            if (!evn.IsLastItemRemoved) {
                return;
            }
            var recipe = _craftConfig.FindRecipe(evn.ItemId.FullName);
            if (recipe == null) {
                return;
            }
            foreach (var recipeIngredient in recipe.Ingredients) {
                RespawnLoot(recipeIngredient.Id);
            }

        }

        private void TryIncreaseRespawnAmount(ItemChangedEvent evn)
        {
            if (!_respawnLoots.ContainsKey(evn.ItemId.FullName)) {
                return;
            }
            if (!evn.IsItemDecreased) {
                return;
            }
            var respawnLootInfo = _respawnLoots[evn.ItemId.FullName];
            var respawnAmountIncrement = evn.PreviousAmount - evn.CurrentAmount;
            var respawnAmount = Math.Min(respawnLootInfo.RespawnAmount + respawnAmountIncrement, respawnLootInfo.CurrentReceivedItemCount);
            respawnLootInfo.RespawnAmount = respawnAmount;
        }

 
        private void RespawnLoot(string itemId)
        {
            if (!_respawnLoots.ContainsKey(itemId)) { 
                return;
            }
            var respawnLootInfo = _respawnLoots[itemId];
            while (respawnLootInfo.RespawnAmount > 0) {
                var respawnLoot = respawnLootInfo.LootQueue.Dequeue();
                _lootFactory.CreateLoot(respawnLoot);
                respawnLootInfo.RespawnAmount = Math.Max(respawnLootInfo.RespawnAmount - respawnLoot.ReceivedItem.Amount, 0);
            }
        }
        public void OnWorldSetup()
        {
            Dispose();
        }

        public void OnWorldCleanUp()
        {
            Dispose();
        }

        private void Dispose() => _respawnLoots.Clear();
    }
}