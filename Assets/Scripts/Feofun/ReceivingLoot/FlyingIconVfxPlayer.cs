using System.Collections;
using System.Linq;
using Feofun.ReceivingLoot.Config;
using Feofun.ReceivingLoot.Model;
using Feofun.ReceivingLoot.View;
using Feofun.UI.Loader;
using UnityEngine;

namespace Feofun.ReceivingLoot
{
    public class FlyingIconVfxPlayer : MonoBehaviour
    {
        private static readonly int[] _lootCounts = {1, 2, 3, 5, 10, 50, 100, 200, 400, 1000, 2000};

        private UILoader _uiLoader;
        private FlyingIconVfxConfig _vfxConfig;
        private Transform _lootContainer;

        public FlyingIconVfxPlayer Init(UILoader uiLoader, FlyingIconVfxConfig vfxConfig, Transform lootContainer)
        {
            _uiLoader = uiLoader;
            _vfxConfig = vfxConfig;
            _lootContainer = lootContainer;
            return this;
        }

        public void Play(FlyingIconVfxParams vfxParams)
        {
            int lootCount = GetDisplayedItemCount(vfxParams.Count);
            StartCoroutine(CreateLootVfx(vfxParams, lootCount));
        }

        private IEnumerator CreateLootVfx(FlyingIconVfxParams vfxParams, int lootCount)
        {
            for (int i = 0; i < lootCount; i++) {
                CreateVfxItem(vfxParams, lootCount);
                yield return new WaitForSeconds(_vfxConfig.CreateDelay);
            }
        }

        private void CreateVfxItem(FlyingIconVfxParams vfxParams, int lootCount)
        {
            var startPosition = vfxParams.StartPosition;
            if (lootCount > 1) {
                startPosition = vfxParams.StartPosition + GetRandomGlobalOffset();
            }
            var model = FlyingIconViewModel.Create(vfxParams, _vfxConfig, startPosition);
            CreateVfxItemView(model);
        }

        private void CreateVfxItemView(FlyingIconViewModel viewModel)
        {
            _uiLoader.Instance(UIModel<FlyingIconView, FlyingIconViewModel>.Create(viewModel)
                                                                             .Container(_lootContainer)
                                                                             .Prefab(_vfxConfig.InstancePrefab));
        }

        private Vector2 GetRandomGlobalOffset() =>
                new Vector2(Random.Range(-_vfxConfig.CreateDispersionX, _vfxConfig.CreateDispersionX),
                            Random.Range(-_vfxConfig.CreateDispersionY, _vfxConfig.CreateDispersionY));

        private int GetDisplayedItemCount(int lootCount) => _lootCounts.Count(it => it <= lootCount);
    }
}