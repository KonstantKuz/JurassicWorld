using UnityEngine;
using UnityEngine.UI;

namespace Feofun.Tutorial.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class TutorialUiElement : MonoBehaviour
    {
        [SerializeField]
        private string _id;
        [SerializeField]
        private Transform _customPointerPosition;
        
        private void Awake()
        {
            TutorialUiElementObserver.Add(this);
            AddClickListener();
        }
        private void AddClickListener()
        {
            var button = GetComponentInChildren<Button>();
            if (button == null) return;
            button.onClick.AddListener(OnClick);
        }
        private void OnClick() => TutorialUiElementObserver.DispatchOnClicked(this);
        private void OnEnable() => TutorialUiElementObserver.DispatchOnActivated(this);
        private void OnDestroy() => TutorialUiElementObserver.Remove(this);
        public string Id
        {
            get => _id;
            set
            {
                TutorialUiElementObserver.Remove(this);
                _id = value;
                TutorialUiElementObserver.Add(this);
                if (gameObject.activeSelf) {
                    TutorialUiElementObserver.DispatchOnActivated(this);
                }
            }
        }

        public Transform PointerPosition => _customPointerPosition ? _customPointerPosition : GetComponent<RectTransform>().transform;
    }
}