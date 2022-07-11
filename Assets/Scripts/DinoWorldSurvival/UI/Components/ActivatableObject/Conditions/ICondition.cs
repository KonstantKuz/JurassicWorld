using System;

namespace DinoWorldSurvival.UI.Components.ActivatableObject.Conditions
{
    public interface ICondition
    {
        IObservable<bool> IsAllow();
    }
}