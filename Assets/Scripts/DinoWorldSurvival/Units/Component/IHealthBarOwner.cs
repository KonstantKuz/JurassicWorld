using UniRx;

namespace DinoWorldSurvival.Units.Component
{
    public interface IHealthBarOwner
    {
        float StartingMaxValue { get; }
        IReadOnlyReactiveProperty<float> MaxValue { get; }
        IReadOnlyReactiveProperty<float> CurrentValue { get; }
    }
}