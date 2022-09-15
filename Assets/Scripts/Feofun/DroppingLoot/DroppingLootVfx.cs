using System.Collections;
using System.Linq;
using Feofun.DroppingLoot.Component;
using Feofun.DroppingLoot.Config;
using Feofun.DroppingLoot.Model;
using Feofun.UI.Loader;
using UnityEngine;

namespace LegionMaster.DroppingLoot
{
    public class DroppingLootVfx : MonoBehaviour
    {
        private static readonly int[] _lootCounts = {1, 2, 3, 5, 10, 50, 100, 200, 400, 1000, 2000};

        private UILoader _uiLoader;
        private DroppingLootConfig _config;     
        private GameObject _lootContainer;

        public DroppingLootVfx Init(UILoader uiLoader, DroppingLootConfig config, GameObject lootContainer)
        {
            _uiLoader = uiLoader;
            _config = config;
            _lootContainer = lootContainer;
            return this;
        }

        public void Play(DroppingLootModel loot)
        {
            int lootCount = GetDisplayedItemCount(loot.Count);
            AddLoot(loot, lootCount);
        }

        private void AddLoot(DroppingLootModel droppingLoot, int lootCount)
        {
            if (lootCount == 0) {
                return;
            }
            StartCoroutine(CreateLoot(droppingLoot, lootCount));
        }

        private IEnumerator CreateLoot(DroppingLootModel loot, int lootCount)
        {
            var startPosition = loot.StartPosition + GetRandomGlobalOffset();
            var model = DroppingObjectModel.Create(loot.Type, _config, startPosition, loot.FinishPosition);
            CreateDroppingObject(model);
            
            yield return new WaitForSeconds(_config.CreateDelay);
            lootCount--;
            AddLoot(loot, lootCount);
        }
        private void CreateDroppingObject(DroppingObjectModel model)
        {
            _uiLoader.Instance(UIModel<DroppingObject, DroppingObjectModel>
                               .Create(model)
                               .Container(_lootContainer.transform)
                               .Prefab(_config.InstancePrefab));
        }
        private Vector2 GetRandomGlobalOffset() =>
                new Vector2(Random.Range(-_config.CreateDispersionX, _config.CreateDispersionX),
                            Random.Range(-_config.CreateDispersionY, _config.CreateDispersionY));

        private int GetDisplayedItemCount(int lootCount) => _lootCounts.Count(it => it <= lootCount);
    }
}