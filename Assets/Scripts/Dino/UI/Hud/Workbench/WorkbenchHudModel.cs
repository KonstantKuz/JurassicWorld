using System;

namespace Dino.UI.Hud.Workbench
{
    public class WorkbenchHudModel
    {
        public readonly bool CanCraft;

        public readonly Action OnCraft;

        public WorkbenchHudModel(bool canCraft, Action onCraft)
        {
            CanCraft = canCraft;
            OnCraft = onCraft;
        }
    }
}