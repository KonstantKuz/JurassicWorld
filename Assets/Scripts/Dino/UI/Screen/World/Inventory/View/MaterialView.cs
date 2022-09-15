using Dino.UI.Screen.World.Inventory.Model;
using Dino.Util;
using Feofun.UI.Components;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Dino.UI.Screen.World.Inventory.View
{
    public class MaterialView : MonoBehaviour, IUiInitializable<MaterialViewModel>
    {
        [SerializeField]
        private Image _image;
        [SerializeField]
        private TextMeshProUGUI _amountText;

        public void Init(MaterialViewModel model)
        {
            _image.sprite = Resources.Load<Sprite>(IconPath.GetInventory(model.Id));
            _amountText.SetText(model.Amount.ToString());
        }
    }
}