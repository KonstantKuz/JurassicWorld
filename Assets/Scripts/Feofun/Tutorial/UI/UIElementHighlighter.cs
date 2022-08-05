using UnityEngine;
using UnityEngine.UI;

namespace Feofun.Tutorial.UI
{
    public class UIElementHighlighter : MonoBehaviour
    {
        [SerializeField] private GameObject _testObject;

        [SerializeField] private Color _backgroundColor;
        [SerializeField] private Image _background; 

        private const int SORT_ORDER = 1;
        private GameObject _highlightedObject;

        public GameObject Background => _background.gameObject;

        private void OnEnable()
        {
            if (_testObject != null)
            {
                Set(_testObject); //test code
            }
        }
        public void Set(Component component, bool showBackground = true) => Set(component.gameObject, showBackground);

        public void Set(GameObject uiElement, bool showBackground = true)
        {
            if (_highlightedObject != null)
            {
                Clear();
            }

            _highlightedObject = uiElement;
            if (uiElement.TryGetComponent(out Canvas canvas))
            {
                Debug.LogError($"Object {_highlightedObject.name} already has canvas");
                return;
            }
            AddHighlight(uiElement);
            Background.SetActive(true);
            _background.color = showBackground ? _backgroundColor : Color.clear;
        }
        
        public void Clear()
        {
            if (_highlightedObject == null) {
                Background.SetActive(false);
                return;
            }
            var canvas = _highlightedObject.GetComponent<Canvas>();
            if (canvas == null) {
                Debug.LogError($"Canvas was already removed from {_highlightedObject.name}");
            } else {
                RemoveHighlight(canvas);
            }
            _highlightedObject = null;            
        }

        private void AddHighlight(GameObject uiElement)
        {
            var canvas = uiElement.AddComponent<Canvas>();
            uiElement.AddComponent<GraphicRaycaster>();
            canvas.overrideSorting = true;
            canvas.sortingOrder = SORT_ORDER + 1;
        }
        private void RemoveHighlight(Canvas canvas)
        {
            Destroy(_highlightedObject.GetComponent<GraphicRaycaster>());
            Destroy(canvas);
            Background.SetActive(false);
        }
    }
}