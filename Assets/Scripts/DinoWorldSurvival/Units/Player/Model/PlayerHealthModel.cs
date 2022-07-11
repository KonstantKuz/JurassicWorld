using DinoWorldSurvival.Modifiers;
using DinoWorldSurvival.Units.Model;
using Feofun.Modifiers;
using Feofun.Modifiers.Parameters;
using UniRx;

namespace DinoWorldSurvival.Units.Player.Model
{
    public class PlayerHealthModel : IHealthModel
    {
        private readonly FloatModifiableParameter _maxHealth;

        public PlayerHealthModel(float maxHealth, IModifiableParameterOwner parameterOwner)
        {
            StartingMaxHealth = maxHealth;
            _maxHealth = new FloatModifiableParameter(Parameters.HEALTH, maxHealth, parameterOwner);
        }
        public float StartingMaxHealth { get; }
        public IReadOnlyReactiveProperty<float> MaxHealth => _maxHealth.ReactiveValue;
    }
}