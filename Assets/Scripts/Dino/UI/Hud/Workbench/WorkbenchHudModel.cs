using System;
using Dino.Inventory.Service;
using UniRx;

namespace Dino.UI.Hud.Workbench
{
    public class WorkbenchHudModel
    {
        private readonly BoolReactiveProperty _craftAvailable;
        private readonly BoolReactiveProperty _craftButtonShown;
        private readonly CraftService _craftService;
        
      
        public readonly Action OnCraft;
        public IReadOnlyReactiveProperty<bool> CraftButtonShown => _craftButtonShown;
        public IReadOnlyReactiveProperty<bool> CraftAvailable => _craftAvailable;
        public Location.Workbench.Workbench Workbench { get; }
        public string CraftItemId => Workbench.CraftItemId;
   
        
        public WorkbenchHudModel(Location.Workbench.Workbench workbench, CraftService craftService, Action onCraft)
        {
            Workbench = workbench;
            _craftService = craftService;
            _craftAvailable = new BoolReactiveProperty(CanCraftRecipe());
            _craftButtonShown = new BoolReactiveProperty(Workbench.IsPlayerInCraftingArea);
            OnCraft = onCraft;
        }
        public void Update()
        {
            _craftAvailable.SetValueAndForceNotify(CanCraftRecipe());
            _craftButtonShown.SetValueAndForceNotify(Workbench.IsPlayerInCraftingArea);
        }
        public bool CanCraftRecipe() => _craftService.HasIngredientsForRecipe(CraftItemId);
    }
}