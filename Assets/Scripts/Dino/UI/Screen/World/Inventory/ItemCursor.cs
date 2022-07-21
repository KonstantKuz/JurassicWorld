using Feofun.Extension;
using UnityEngine;
using UnityEngine.Assertions;

namespace Dino.UI.Screen.World.Inventory
{
    public class ItemCursor : MonoBehaviour
    {
        [SerializeField]
        private Transform _cursorRoot;

        public GameObject AttachedItem { get; private set; }

        private Transform CursorRoot => _cursorRoot;

        public bool IsItemAttached => AttachedItem != null;
        

        public void Attach(GameObject item)
        {
            Assert.IsNull(AttachedItem);
            AttachedItem = item;
            item.transform.SetParent(_cursorRoot, false);
            item.transform.ResetLocalTransform();
        }

        private void Update()
        {
            if(AttachedItem == null) return;
            CursorRoot.position = Input.mousePosition;
        }

        public void Detach()
        {
            if (AttachedItem == null) return;
            Destroy(AttachedItem);
            AttachedItem = null;
         
        }

    }
}