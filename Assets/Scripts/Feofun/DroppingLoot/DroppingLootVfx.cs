using System.Collections;
using System.Linq;
using Feofun.DroppingLoot.Config;
using Feofun.DroppingLoot.Model;
using Feofun.DroppingLoot.View;
using Feofun.UI.Loader;
using UnityEngine;

namespace Feofun.DroppingLoot
{
    public class DroppingLootVfx : MonoBehaviour
    {
        private static readonly int[] _lootCounts = {1, 2, 3, 5, 10, 50, 100, 200, 400, 1000, 2000};

        private UILoader _uiLoader;
        private DroppingLootConfig _config;     
        private Transform _lootContainer;

        public DroppingLootVfx Init(UILoader uiLoader, DroppingLootConfig config, Transform lootContainer)
        {
            _uiLoader = uiLoader;
            _config = config;
            _lootContainer = lootContainer;
            return this;
        }

        public void Play(DroppingLootInitParams initParams)
        {
            int lootCount = GetDisplayedItemCount(initParams.Count);
            AddLoot(initParams, lootCount);
        }

        private void AddLoot(DroppingLootInitParams initParams, int lootCount)
        {
            if (lootCount == 0) {
                return;
            }
            StartCoroutine(CreateLoot(initParams, lootCount));
        }

        private IEnumerator CreateLoot(DroppingLootInitParams initParams, int lootCount)
        {
            var startPosition = initParams.StartPosition;
            if (lootCount != 1) {
                startPosition = initParams.StartPosition + GetRandomGlobalOffset();
            }
            var model = DroppingObjectViewModel.Create(initParams, _config, startPosition);
            CreateDroppingObject(model);
            
            yield return new WaitForSeconds(_config.CreateDelay);
            lootCount--;
            AddLoot(initParams, lootCount);
        }
        private void CreateDroppingObject(DroppingObjectViewModel viewModel)
        {
            _uiLoader.Instance(UIModel<DroppingObjectView, DroppingObjectViewModel>
                               .Create(viewModel)
                               .Container(_lootContainer)
                               .Prefab(_config.InstancePrefab));
        }
        private Vector2 GetRandomGlobalOffset() =>
                new Vector2(Random.Range(-_config.CreateDispersionX, _config.CreateDispersionX),
                            Random.Range(-_config.CreateDispersionY, _config.CreateDispersionY));

        private int GetDisplayedItemCount(int lootCount) => _lootCounts.Count(it => it <= lootCount);
    }
}