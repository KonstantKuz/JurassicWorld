using Dino.Units.Component.Target;
using JetBrains.Annotations;

namespace Dino.Units.Component.TargetSearcher
{
    public interface ITargetSearcher
    {
        [CanBeNull]
        ITarget Find(float searchDistance);
    }
}