using DinoWorldSurvival.Location.Model;
using DinoWorldSurvival.Loot.Config;
using UnityEngine;

namespace DinoWorldSurvival.Loot
{
    public class DroppingLoot : WorldObject
    {
        public DroppingLootConfig Config { get; private set; }
        public void Init(DroppingLootConfig config)
        {
            Config = config;
            SetRandomRotation();
        }

        private void SetRandomRotation()
        {
            transform.localRotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);
        }
    }
}