using System.Collections.Generic;
using System.Linq;
using ModestTree;
using UnityEngine;
using UnityEngine.UI;

namespace Feofun.Tutorial.UI
{
    public class UIElementHighlighter : MonoBehaviour
    {
        [SerializeField] private GameObject _testObject;

        [SerializeField] private Color _backgroundColor;
        [SerializeField] private Image _background; 

        private readonly List<GameObject> _highlightedObjects = new List<GameObject>();

        public GameObject Background => _background.gameObject;

        private void OnEnable()
        {
            if (_testObject != null)
            {
                Set(_testObject); //test code
            }
        }
        public void Set(Component component, bool showBackground = true) => Set(component.gameObject, showBackground);
        
        public void Set(IEnumerable<Component> component, bool showBackground = true) => Set(component.Select(it => it.gameObject).ToArray(), showBackground);

        public void Set(IEnumerable<GameObject> uiElements, bool showBackground = true)
        {
            if (!_highlightedObjects.IsEmpty())
            {
                Clear();
            }

            foreach (var obj in uiElements)
            {
                if (obj.TryGetComponent(out Canvas canvas))
                {
                    Debug.LogError($"Object {obj.name} already has canvas");
                    continue;
                }
                AddHighlight(obj);
            }
            Background.SetActive(true);
            _background.color = showBackground ? _backgroundColor : Color.clear;
        }

        public void Set(GameObject uiElement, bool showBackground = true) => Set(new[] { uiElement }, showBackground);
        
        public void Clear()
        {
            foreach (var obj in _highlightedObjects)
            {
                var canvas = obj.GetComponent<Canvas>();
                if (canvas == null) {
                    Debug.LogError($"Canvas was already removed from {obj.name}");
                } else {
                    Destroy(obj.GetComponent<GraphicRaycaster>());
                    Destroy(canvas);
                }
            }
            _highlightedObjects.Clear();   
            Background.SetActive(false);
        }

        private void AddHighlight(GameObject uiElement)
        {
            var canvas = uiElement.AddComponent<Canvas>();
            uiElement.AddComponent<GraphicRaycaster>();
            canvas.overrideSorting = true;
            canvas.sortingOrder = canvas.rootCanvas.sortingOrder + 1;
            
            _highlightedObjects.Add(uiElement);
        }
    }
}