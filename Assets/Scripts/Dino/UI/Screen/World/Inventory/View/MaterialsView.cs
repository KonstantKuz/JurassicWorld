using System;
using System.Collections.Generic;
using Dino.UI.Screen.World.Inventory.Model;
using Feofun.Extension;
using SuperMaxim.Core.Extensions;
using UniRx;
using UnityEngine;
using Zenject;

namespace Dino.UI.Screen.World.Inventory.View
{
    public class MaterialsView : MonoBehaviour
    {
        [SerializeField] 
        private Transform _root;
        [SerializeField] 
        private MaterialView _materialPrefab;

        private IDisposable _disposable;
        
        [Inject]
        private DiContainer _container;
        
        public void Init(MaterialsModel model)
        {
            Dispose();
            _disposable = model.Materials.Subscribe(UpdateMaterials);
        }

        private void UpdateMaterials(IReadOnlyList<MaterialViewModel> materialViews)
        {
            _root.DestroyAllChildren();
            materialViews.ForEach(it =>
            {
                var materialView = _container.InstantiatePrefabForComponent<MaterialView>(_materialPrefab, _root);
                materialView.Init(it);
            });
        }

        private void Dispose()
        {
            _root.DestroyAllChildren();
            _disposable?.Dispose();
            _disposable = null;
        }

        private void OnDisable()
        {
            Dispose();
        }
    }
}
