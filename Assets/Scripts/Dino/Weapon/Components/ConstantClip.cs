using UniRx;

namespace Dino.Weapon.Components
{
    public class ConstantClip : IClip
    {
        private const int CONSTANT_AMMO_COUNT = 1;
        public bool HasAmmo => true;
        public IReactiveProperty<int> AmmoCount { get; }
        public void OnFire() { }

        public ConstantClip()
        {
            AmmoCount = new ReactiveProperty<int>(CONSTANT_AMMO_COUNT);
        }
    }
}