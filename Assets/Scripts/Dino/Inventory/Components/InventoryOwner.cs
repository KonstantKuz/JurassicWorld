using Feofun.Extension;
using UnityEngine;

namespace Dino.Inventory.Components
{
    public class InventoryOwner : MonoBehaviour
    {
        [SerializeField]
        private Transform _container;
        
        public Transform Container => _container;

        public void Set(GameObject item)
        {
            item.transform.SetParent(Container);
            item.transform.ResetLocalTransform();
        }
        public void Delete()
        {
            _container.DetachChildren();
        }
    }
}