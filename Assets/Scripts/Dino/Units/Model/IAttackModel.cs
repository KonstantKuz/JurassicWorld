using UniRx;

namespace Dino.Units.Model
{
    public interface IAttackModel
    {
        float TargetSearchRadius { get; }
        float AttackDistance { get; }
        float AttackDamage { get; }
        public IReadOnlyReactiveProperty<float> AttackInterval { get; }
    }
}