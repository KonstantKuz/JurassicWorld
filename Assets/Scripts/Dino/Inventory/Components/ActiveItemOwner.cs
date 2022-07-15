using System;
using Feofun.Extension;
using JetBrains.Annotations;
using ModestTree;
using UnityEngine;

namespace Dino.Inventory.Components
{
    public class ActiveItemOwner : MonoBehaviour
    {
        [SerializeField]
        private Transform _container;

        private GameObject _currentItem;

        [CanBeNull]
        public GameObject CurrentItem
        {
            get
            {
                if (_currentItem == null) {
                    throw new NullReferenceException("InventoryItem is null, should set inventory item");
                }
                return _currentItem;
            }
            private set => _currentItem = value;
        }
        public Transform Container => _container;

        public void Set(GameObject item)
        {
            Assert.IsNull(_currentItem, "Player inventory item is not null, should delete the previous inventory item");
            CurrentItem = item;
            item.transform.SetParent(Container);
            item.transform.ResetLocalTransform();
        }

        public void Remove()
        {
            CurrentItem = null;
            _container.DestroyAllChildren();
        }
    }
}