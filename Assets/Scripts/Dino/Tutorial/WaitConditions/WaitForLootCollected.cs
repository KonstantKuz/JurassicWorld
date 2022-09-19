using System.Collections.Generic;
using UnityEngine;

namespace Dino.Tutorial.WaitConditions
{
    public class WaitForLootCollected : CustomYieldInstruction
    {
        private bool _lootCollected;
        public override bool keepWaiting => !_lootCollected;

        public WaitForLootCollected(Loot.Loot loot)
        {
            loot.OnCollected += OnLootCollected;
        }

        private void OnLootCollected(Loot.Loot loot)
        {
            loot.OnCollected -= OnLootCollected;
            _lootCollected = true;
        }
    }
}