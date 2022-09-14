using UnityEngine;

namespace Dino.UI.Hud.Workbench
{
    public class WorkbenchHudPresenter : MonoBehaviour
    {
        [SerializeField] private WorkbenchHudView _view;
        
        private WorkbenchHudModel _model;
        
        public void Init(Location.Workbench.Workbench workbench)
        {
            Dispose();
            _model = new WorkbenchHudModel(workbench);
            _view.Init(_model);

        }
        private void OnDestroy()
        {
            Dispose();
        }
        private void Dispose()
        {
            _model?.Dispose();
            _model = null;
        }
    }
}