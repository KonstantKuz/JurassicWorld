using UniRx;

namespace Dino.Units.Model
{
    public interface IHealthModel
    { 
        float StartingMaxHealth { get; }
        IReadOnlyReactiveProperty<float> MaxHealth { get; }
    }
}