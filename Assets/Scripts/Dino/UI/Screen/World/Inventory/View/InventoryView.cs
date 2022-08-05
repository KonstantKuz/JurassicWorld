using System.Collections.Generic;
using System.Linq;
using Dino.Inventory.Config;
using Dino.UI.Screen.World.Inventory.Model;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace Dino.UI.Screen.World.Inventory.View
{
    public class InventoryView : MonoBehaviour
    {
        [SerializeField]
        private InventoryItemView _itemPrefab;
        private InventoryItemView[] _items;
        
        private CompositeDisposable _disposable;

        public InventoryItemView ItemPrefab => _itemPrefab;

        private InventoryItemView[] Items => _items ??= GetComponentsInChildren<InventoryItemView>();
        
        public void Init(IReactiveProperty<List<ItemViewModel>> items)
        {
            _disposable?.Dispose();
            _disposable = new CompositeDisposable();
            items.Subscribe(UpdateItems).AddTo(_disposable);
        }
        private void UpdateItems(IReadOnlyList<ItemViewModel> items)
        {
            for (int idx = 0; idx < Items.Length; idx++)
            {
                if (idx >= items.Count)
                {
                    _items[idx].gameObject.SetActive(false);
                }
                else
                {
                    _items[idx].gameObject.SetActive(true);
                    _items[idx].Init(items[idx]);
                }
            }
        }

        private void OnDisable()
        {
            _disposable?.Dispose();
            _disposable = null;
        }

        public InventoryItemView GetItemView(IngredientConfig ingredientConfig)
        {
            var item = Items.FirstOrDefault(it => it.gameObject.activeSelf && it.Model?.Id?.Name == ingredientConfig.Name);
            Assert.IsTrue(item);
            return item;
        }
    }
}