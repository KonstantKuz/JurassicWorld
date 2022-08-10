using System.Collections.Generic;

namespace Feofun.Tutorial.UI
{
    public delegate void UiElementDelegate(TutorialUiElement obj);
    
    public static class TutorialUiElementObserver
    {
        private static readonly Dictionary<string, TutorialUiElement> _elements = new Dictionary<string, TutorialUiElement>();
        
        public static event UiElementDelegate OnElementActivated;
        public static event UiElementDelegate OnElementClicked;

        public static void Add(TutorialUiElement element)
        {
            if (_elements.ContainsKey(element.Id)) {
                return;
            }
            _elements[element.Id] = element;
        }       
        public static void Remove(TutorialUiElement element)
        {
            if (!_elements.ContainsKey(element.Id)) {
                return;
            }
            _elements.Remove(element.Id);
        }

        public static void DispatchOnActivated(TutorialUiElement element)
        { 
            OnElementActivated?.Invoke(element);
        }
        
        public static void DispatchOnClicked(TutorialUiElement element)
        {
            OnElementClicked?.Invoke(element);
        }

        public static TutorialUiElement Get(string id) => _elements[id];
        
        public static bool Contains(string id) => _elements.ContainsKey(id);

    }
}