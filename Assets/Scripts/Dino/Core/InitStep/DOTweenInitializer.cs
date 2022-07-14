using DG.Tweening;

namespace Dino.Core.InitStep
{
    public static class DOTweenInitializer
    {
        private const int TWEENERS_CAPACITY = 500;
        private const int SEQUENCES_CAPACITY = 312;

        public static void Init()
        {
            DOTween.SetTweensCapacity(TWEENERS_CAPACITY, SEQUENCES_CAPACITY);
        }
    }
}