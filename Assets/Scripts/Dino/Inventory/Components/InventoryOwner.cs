using UnityEngine;

namespace Dino.Inventory.Components
{
    public class InventoryOwner : MonoBehaviour
    {
        [SerializeField]
        private Transform Container;


        public void SetInventory(GameObject item)
        {
            item.transform.SetParent(Container);
            item.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        }
    }
}