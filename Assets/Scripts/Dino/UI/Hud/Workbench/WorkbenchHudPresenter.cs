using Feofun.Extension;
using Logger.Extension;
using UnityEngine;

namespace Dino.UI.Hud.Workbench
{
    public class WorkbenchHudPresenter : MonoBehaviour
    {
        [SerializeField] private WorkbenchHudView _view;
        
        private Transform _hudPlace;
        private Location.Workbench.Workbench _workbench;
        
        private WorkbenchHudModel _model;

        public void Init(Location.Workbench.Workbench workbench)
        {
            _model = new WorkbenchHudModel(workbench.CanCraft(), OnCraft);
            _view.Init(_model);
        }
        private void OnCraft()
        {
            if (!_workbench.CanCraft()) {
                this.Logger().Error($"Recipe crafting error, invalid ingredients, receptId:= {_workbench.CraftReceptId}");
                return;
            }
            _workbench.Craft();
        }
        private void Update()
        {
            if (_hudPlace != null) {
                transform.position = _hudPlace.WorldToScreenPoint();
            }
        }
        public void OnDestroy()
        {
            _model = null;
        }
    }
}