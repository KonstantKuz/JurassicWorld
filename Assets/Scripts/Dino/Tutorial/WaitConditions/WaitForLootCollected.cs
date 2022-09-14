using System.Collections.Generic;
using UnityEngine;

namespace Dino.Tutorial.WaitConditions
{
    public class WaitForLootCollected : CustomYieldInstruction
    {
        private readonly List<Loot.Loot> _loots;
        private bool _lootCollected;
        public override bool keepWaiting => !_lootCollected;

        public WaitForLootCollected(List<Loot.Loot> loots)
        {
            _loots = loots;
            _loots.ForEach(it => it.OnCollected += OnLootCollected);
        }

        private void OnLootCollected(Loot.Loot loot)
        {
            loot.OnCollected -= OnLootCollected;
            
            _loots.Remove(loot);
            if (_loots.Count == 0)
            {
                _lootCollected = true;
            }
        }
    }
}