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
        private InventoryModel _model;

        public InventoryItemView ItemPrefab => _itemPrefab;

        private InventoryItemView[] Items => _items ??= GetComponentsInChildren<InventoryItemView>();

        public void Init(InventoryModel model)
        {
            Dispose();
            _disposable = new CompositeDisposable();
            _model = model;
            model.Items.Subscribe(UpdateItems).AddTo(_disposable);
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

        private void Dispose()
        {
            _disposable?.Dispose();
            _disposable = null;
            _model = null;
        }

        private void OnDisable()
        {
            Dispose();
        }

        public IEnumerable<InventoryItemView> GetItemViews(string itemName)
        {
            var itemId = ItemId.Create(itemName, _model.InventoryType);
            return Items
                .Where(it => it.gameObject.activeSelf && itemId.Equals(it.Model?.Id));
        }
    }
}