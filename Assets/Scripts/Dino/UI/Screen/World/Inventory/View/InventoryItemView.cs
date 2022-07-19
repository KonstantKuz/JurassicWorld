using Dino.UI.Screen.World.Inventory.Model;
using Feofun.UI.Components.Button;
using UnityEngine;
using UnityEngine.UI;

namespace Dino.UI.Screen.World.Inventory.View
{
    public class InventoryItemView : MonoBehaviour
    {
        
        [SerializeField] private Image _icon;
        [SerializeField] private ActionButton _button;
        
        
        public void Init(ItemViewModel model)
        {
            if (model.Item == null) {
                return;
            }
            //_icon.sprite = Resources.Load<Sprite>(IconPath.GetUpgrade(model.Item.Id));
            _button.Init(model.OnClick);
        }
    }
}