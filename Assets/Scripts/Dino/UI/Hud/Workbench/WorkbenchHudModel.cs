using System;
using UniRx;

namespace Dino.UI.Hud.Workbench
{
    public class WorkbenchHudModel
    {
        private readonly BoolReactiveProperty _craftAvailable;
        private readonly BoolReactiveProperty _craftButtonShown;

      
        public readonly Action OnCraft;
        public IReadOnlyReactiveProperty<bool> CraftButtonShown => _craftButtonShown;
        public IReadOnlyReactiveProperty<bool> CraftAvailable => _craftAvailable;
        public Location.Workbench.Workbench Workbench { get; }

        public string CraftRecipeId => Workbench.CraftRecipeId;

        public WorkbenchHudModel(Location.Workbench.Workbench workbench, Action onCraft)
        {
            Workbench = workbench;
            _craftAvailable = new BoolReactiveProperty(workbench.CanCraftRecipe());
            _craftButtonShown = new BoolReactiveProperty(Workbench.IsPlayerInCraftingArea);
            OnCraft = onCraft;
        }
        public void Update()
        {
            _craftAvailable.SetValueAndForceNotify(Workbench.CanCraftRecipe());
            _craftButtonShown.SetValueAndForceNotify(Workbench.IsPlayerInCraftingArea);
        }
    }
}