using UniRx;

namespace Dino.Weapon.Components
{
    public class InfiniteClip : IClip
    {
        private const int INFINITE_AMMO_COUNT = 100000;
        public bool HasAmmo => true;
        public IReactiveProperty<int> AmmoCount { get; }
        public void OnFire() { }

        public InfiniteClip()
        {
            AmmoCount = new ReactiveProperty<int>(INFINITE_AMMO_COUNT);
        }
    }
}