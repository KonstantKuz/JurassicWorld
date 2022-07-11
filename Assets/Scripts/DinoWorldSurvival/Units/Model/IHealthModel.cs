using UniRx;

namespace DinoWorldSurvival.Units.Model
{
    public interface IHealthModel
    { 
        float StartingMaxHealth { get; }
        IReadOnlyReactiveProperty<float> MaxHealth { get; }
    }
}