using Feofun.Tutorial.UI;
using UnityEngine;

namespace Dino.Tutorial
{
    public class TutorialUiTools : MonoBehaviour
    {
        [SerializeField]
        private UIElementHighlighter _elementHighlighter;

        public UIElementHighlighter ElementHighlighter => _elementHighlighter;
    }
}