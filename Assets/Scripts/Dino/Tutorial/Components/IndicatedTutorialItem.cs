using UnityEngine;

namespace Dino.Tutorial.Components
{
    public class IndicatedTutorialItem : MonoBehaviour
    {
        [SerializeField] private string _itemId;
        public string ItemId => _itemId;
    }
}