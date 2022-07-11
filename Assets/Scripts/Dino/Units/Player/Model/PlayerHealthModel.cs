using Dino.Modifiers;
using Dino.Units.Model;
using Feofun.Modifiers;
using Feofun.Modifiers.Parameters;
using UniRx;

namespace Dino.Units.Player.Model
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