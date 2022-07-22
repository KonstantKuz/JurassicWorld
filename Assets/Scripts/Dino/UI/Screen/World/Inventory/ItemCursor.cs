using System.Collections.Generic;
using System.Linq;
using Feofun.Extension;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using ModestTree;
using Assert = UnityEngine.Assertions.Assert;

namespace Dino.UI.Screen.World.Inventory
{
    public class ItemCursor : MonoBehaviour
    {
        [SerializeField]
        private Transform _cursorRoot;

        public GameObject AttachedItem { get; private set; }

        private Transform CursorRoot => _cursorRoot;
        
        
        public void Attach(GameObject item)
        {
            Assert.IsNull(AttachedItem);
            AttachedItem = item;
            item.transform.SetParent(_cursorRoot, false);
            item.transform.ResetLocalTransform();
        }
        public void Detach()
        {
            if (AttachedItem == null) return;
            Destroy(AttachedItem);
            AttachedItem = null;
         
        }
        private void Update()
        {
            if (AttachedItem == null) return;
            CursorRoot.position = Input.mousePosition;
        }
        [CanBeNull]
        public T FindComponentUnderCursor<T>()
        {
            var raycastGameObjects = RaycastMouse()
                                     .Select(it => it.gameObject)
                                     .Except(AttachedItem);
            return raycastGameObjects.Select(it => it.GetComponent<T>()).FirstOrDefault(it => it != null);
        }
        public bool IsCursorOverLayer(string layerName)
        {
            return RaycastMouse().Exists(it => it.gameObject.layer == LayerMask.NameToLayer(layerName));
        }
        private List<RaycastResult> RaycastMouse(){
         
            var pointerData = new PointerEventData(EventSystem.current) {
                    pointerId = -1,
                    position = Input.mousePosition,
            };
            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);
            return results;
        }
    }
}