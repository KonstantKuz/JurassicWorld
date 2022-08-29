using Feofun.UI.Components.Button;
using UnityEngine;

namespace Dino.UI.Hud.Workbench
{
    public class WorkbenchHudView : MonoBehaviour
    {
        [SerializeField] private ActionButton _button;

        public void Init(WorkbenchHudModel model)
        {
            _button.Init(model.OnCraft);
            _button.Button.interactable = model.CanCraft;
        }

    }
}