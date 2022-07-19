using System.Linq;
using Dino.UI.Screen.World.Inventory.Model;
using Dino.Util;
using Feofun.UI.Components.Button;
using Feofun.Util.SerializableDictionary;
using Logger.Extension;
using SuperMaxim.Core.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace Dino.UI.Screen.World.Inventory.View
{
    public class InventoryItemView : MonoBehaviour
    {
        [SerializeField]
        private Image _icon;

        [SerializeField]
        private ActionButton _button;
        [SerializeField]
        private SerializableDictionary<ItemViewState, GameObject> _stateContainers;


        public void Init(ItemViewModel model)
        {
            if (model.Item == null) {
                return;
            }
            _icon.sprite = Resources.Load<Sprite>(IconPath.GetInventory(model.Item.Id));
            _button.Init(model.OnClick);
            UpdateState(model.State);
        }

        private void UpdateState(ItemViewState state)
        {
            _stateContainers.Values.ForEach(it => it.SetActive(false));
            if (!_stateContainers.ContainsKey(state)) {
                this.Logger().Error($"State container not found for inventory item state:= {state}");
                return;
            }
            _stateContainers[state].SetActive(true);
        }
    }
}