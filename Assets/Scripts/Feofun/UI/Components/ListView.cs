using System;
using System.Collections.Generic;
using Feofun.Components;
using Feofun.Extension;
using SuperMaxim.Core.Extensions;
using UniRx;
using UnityEngine;
using Zenject;

namespace Feofun.UI.Components
{
    public class ListView : MonoBehaviour
    {
        [SerializeField] 
        private Transform _root;
        [SerializeField] 
        private GameObject _itemPrefab;

        private IDisposable _disposable;
        
        [Inject]
        private DiContainer _container;
        
        public void Init<T>(IReadOnlyReactiveProperty<List<T>> itemModels) where T : class
        {
            Dispose();
            _disposable = itemModels.Subscribe(UpdateMaterials);
        }

        private void UpdateMaterials<T>(IReadOnlyList<T> itemModels) where T : class
        {
            _root.DestroyAllChildren();
            itemModels.ForEach(itemModel =>
            {
                var itemView = _container.InstantiatePrefabForComponent<IInitializable<T>>(_itemPrefab, _root);
                itemView.Init(itemModel);
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
