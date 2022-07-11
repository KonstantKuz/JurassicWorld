using System.Collections.Generic;
using Dino.Units.Target;
using JetBrains.Annotations;

namespace Dino.Units.Component.TargetSearcher
{
    public interface ITargetSearcher
    {
        [CanBeNull]
        ITarget Find();
        // TODO: remove later. used only for multi target ranged weapon for sniper unit.
        IEnumerable<ITarget> GetAllOrderedByDistance();
    }
}