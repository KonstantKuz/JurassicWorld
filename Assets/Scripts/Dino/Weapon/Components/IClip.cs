using UniRx;

namespace Dino.Weapon.Components
{
    public interface IClip
    {
        bool HasAmmo { get; }
        IReactiveProperty<int> AmmoCount { get; }

        void OnFire();
    }
}