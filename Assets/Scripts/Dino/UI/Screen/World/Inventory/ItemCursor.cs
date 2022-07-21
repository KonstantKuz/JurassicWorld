using System.Collections.Generic;
using System.Linq;
using Dino.UI.Screen.World.Inventory.View;
using Feofun.Extension;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Assertions;
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
        [CanBeNull]
        public T FirstComponentByMousePosition<T>()
        {
            var raycastGameObjects = RaycastMouse()
                                     .Select(it => it.gameObject)
                                     .Except(AttachedItem);
            return raycastGameObjects.Select(it => it.GetComponent<T>()).FirstOrDefault(it => it != null);
        }

        public List<RaycastResult> RaycastMouse(){
         
            var pointerData = new PointerEventData(EventSystem.current) {
                    pointerId = -1,
            };
            pointerData.position = Input.mousePosition;
            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);
            return results;
        }
        public void Detach()
        {
            if (AttachedItem == null) return;
            Destroy(AttachedItem);
            AttachedItem = null;
         
        }

    }
}