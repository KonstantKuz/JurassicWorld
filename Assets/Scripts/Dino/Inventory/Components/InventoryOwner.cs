using UnityEngine;

namespace Dino.Inventory.Components
{
    public class InventoryOwner : MonoBehaviour
    {
        [SerializeField]
        private Transform _container;
        
        public Transform Container => _container;

        public void SetInventory(GameObject item)
        {
            item.transform.SetParent(Container);
            item.transform.localPosition = Vector3.zero;
            item.transform.localRotation = Quaternion.identity;
        }
    }
}