using System.Collections.Generic;
using Dino.UI.Screen.World.Inventory.Model;
using Feofun.Extension;
using SuperMaxim.Core.Extensions;
using UniRx;
using UnityEngine;
using Zenject;

namespace Dino.UI.Screen.World.Inventory.View
{
    public class InventoryView : MonoBehaviour
    {
        [SerializeField]
        private InventoryItemView _itemPrefab;
        [SerializeField]
        private Transform _root;

        private CompositeDisposable _disposable;
        
        [Inject] private DiContainer _container;
        
        
        public void Init(IReactiveProperty<List<ItemViewModel>> items)
        {
            _disposable?.Dispose();
            _disposable = new CompositeDisposable();
            RemoveAllCreatedObjects();
            items.Subscribe(UpdateItems).AddTo(_disposable);
        }
        private void UpdateItems(IReadOnlyCollection<ItemViewModel> items)
        {
            RemoveAllCreatedObjects();
            items.ForEach(CreateItem);
        }

        private void CreateItem(ItemViewModel itemViewModel)
        {
            var itemView = _container.InstantiatePrefabForComponent<InventoryItemView>(_itemPrefab, _root);
            itemView.Init(itemViewModel);
        }

        private void OnDisable()
        {
            _disposable?.Dispose();
            _disposable = null;
            RemoveAllCreatedObjects();
        }

        private void RemoveAllCreatedObjects()
        {
            _root.DestroyAllChildren();
        }
    }
}