using JetBrains.Annotations;

namespace Dino.Units.Target
{
    public static class TargetExtension
    {
        public static bool IsTargetValidAndAlive([CanBeNull] this ITarget target)
        {
            return target is {IsAlive: true};
        }
    }
}