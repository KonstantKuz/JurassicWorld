using System;
using UniRx;

namespace Dino.UI.Hud.Workbench
{
    public class WorkbenchHudModel
    {
        private readonly BoolReactiveProperty _canCraft;
        private readonly BoolReactiveProperty _craftButtonShown;

        private Location.Workbench.Workbench _workbench;

        
        public readonly Action OnCraft;
        public IReadOnlyReactiveProperty<bool> CraftButtonShown => _craftButtonShown;
        public IReadOnlyReactiveProperty<bool> CanCraft => _canCraft;
        public Location.Workbench.Workbench Workbench => _workbench;

        public WorkbenchHudModel(Location.Workbench.Workbench workbench, Action onCraft)
        {
            _canCraft = new BoolReactiveProperty(workbench.CanCraftRecipe());
            _craftButtonShown = new BoolReactiveProperty(true);
            OnCraft = onCraft;
        }
        public void Update()
        {
            _canCraft.SetValueAndForceNotify(_workbench.CanCraftRecipe());
        }
    }
}