using System.Collections.Generic;
using System.Linq;
using Dino.Inventory.Model;
using Dino.UI.Screen.World.Inventory.Model;
using UniRx;
using UnityEngine;

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
        private void UpdateItems(IReadOnlyList<ItemViewModel> itemViews)
        {
            for (int idx = 0; idx < Items.Length; idx++)
            {
                if (idx >= itemViews.Count)
                {
                    _items[idx].gameObject.SetActive(false);
                }
                else
                {
                    _items[idx].gameObject.SetActive(true);
                    _items[idx].Init(itemViews[idx]);
                }
            }
        }

        private void OnDisable()
        {
            _disposable?.Dispose();
            _disposable = null;
        }

        public IEnumerable<InventoryItemView> GetItemViews(string itemName)
        {
            var itemId = ItemId.Create(itemName, InventoryItemType.Weapon);

            return Items
                .Where(it => it.gameObject.activeSelf && itemId.Equals(it.Model?.Id));
        }
    }
}