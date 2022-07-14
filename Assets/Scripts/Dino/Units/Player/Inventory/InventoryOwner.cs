using UnityEngine;

namespace Dino.Units.Player.Inventory
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