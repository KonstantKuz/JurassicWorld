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
        private static readonly int[] _iconCounts = {1, 2, 3, 5, 10, 50, 100, 200, 400, 1000, 2000};

        private UILoader _uiLoader;
        private FlyingIconVfxConfig _vfxConfig;
        private Transform _iconContainer;

        public FlyingIconVfxPlayer Init(UILoader uiLoader, FlyingIconVfxConfig vfxConfig, Transform iconContainer)
        {
            _uiLoader = uiLoader;
            _vfxConfig = vfxConfig;
            _iconContainer = iconContainer;
            return this;
        }

        public void Play(FlyingIconVfxParams vfxParams)
        {
            int iconCount = GetDisplayedIconCount(vfxParams.Count);
            StartCoroutine(CreateVfx(vfxParams, iconCount));
        }

        private IEnumerator CreateVfx(FlyingIconVfxParams vfxParams, int iconCount)
        {
            for (int i = 0; i < iconCount; i++) {
                CreateVfxItem(vfxParams, iconCount);
                yield return new WaitForSeconds(_vfxConfig.CreateDelay);
            }
        }

        private void CreateVfxItem(FlyingIconVfxParams vfxParams, int iconCount)
        {
            var startPosition = vfxParams.StartPosition;
            if (iconCount > 1) {
                startPosition = vfxParams.StartPosition + GetRandomGlobalOffset();
            }
            var model = FlyingIconViewModel.Create(vfxParams, _vfxConfig, startPosition);
            CreateVfxItemView(model);
        }

        private void CreateVfxItemView(FlyingIconViewModel viewModel)
        {
            _uiLoader.Instance(UIModel<FlyingIconView, FlyingIconViewModel>.Create(viewModel)
                                                                           .Container(_iconContainer)
                                                                           .Prefab(_vfxConfig.InstancePrefab));
        }

        private Vector2 GetRandomGlobalOffset() =>
                new Vector2(Random.Range(-_vfxConfig.CreateDispersionX, _vfxConfig.CreateDispersionX),
                            Random.Range(-_vfxConfig.CreateDispersionY, _vfxConfig.CreateDispersionY));

        private int GetDisplayedIconCount(int itemCount) => _iconCounts.Count(it => it <= itemCount);
    }
}