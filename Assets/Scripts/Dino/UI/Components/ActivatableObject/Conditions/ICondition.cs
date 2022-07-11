using System;

namespace Dino.UI.Components.ActivatableObject.Conditions
{
    public interface ICondition
    {
        IObservable<bool> IsAllow();
    }
}