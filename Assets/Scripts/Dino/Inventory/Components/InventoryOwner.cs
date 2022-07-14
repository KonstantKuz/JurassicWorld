using System;
using Feofun.Extension;
using JetBrains.Annotations;
using ModestTree;
using UnityEngine;

namespace Dino.Inventory.Components
{
    public class InventoryOwner : MonoBehaviour
    {
        [SerializeField]
        private Transform _container;

        private GameObject _inventoryItem;

        [CanBeNull]
        public GameObject InventoryItem
        {
            get
            {
                if (_inventoryItem == null) {
                    throw new NullReferenceException("InventoryItem is null, should set inventory item");
                }
                return _inventoryItem;
            }
            private set => _inventoryItem = value;
        }
        public Transform Container => _container;

        public void Set(GameObject item)
        {
            Assert.IsNull(_inventoryItem, "Player inventory item is not null, should delete the previous inventory item");
            InventoryItem = item;
            item.transform.SetParent(Container);
            item.transform.ResetLocalTransform();
        }

        public void Remove()
        {
            InventoryItem = null;
            _container.DestroyAllChildren();
        }
    }
}