﻿using System;
using Dino.Extension;
using Dino.Weapon;
using Feofun.Extension;
using JetBrains.Annotations;
using ModestTree;
using UnityEngine;

namespace Dino.Units.Component
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
                    throw new NullReferenceException("Active item is null, should set active item on unit");
                }
                return _currentItem;
            }
            private set => _currentItem = value;
        }
        public Transform Container => _container;

        public void Set(GameObject item)
        {
            Assert.IsNull(_currentItem, "Unit active item is not null, should delete the previous unit active item");
            CurrentItem = item;
            item.transform.SetParent(Container);
            item.transform.ResetLocalTransform();
        }
        public BaseWeapon GetWeapon() => CurrentItem.RequireComponent<BaseWeapon>();

        public void Remove()
        {
            CurrentItem = null;
            _container.DestroyAllChildren();
        }
    }
}