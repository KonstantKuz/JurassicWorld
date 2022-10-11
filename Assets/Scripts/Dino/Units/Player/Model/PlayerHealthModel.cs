using Dino.Modifiers;
using Dino.Units.Model;
using Dino.Units.Player.Config;
using Feofun.Modifiers;
using Feofun.Modifiers.Parameters;
using UniRx;

namespace Dino.Units.Player.Model
{
    public class PlayerHealthModel : IHealthModel
    {
        private readonly FloatModifiableParameter _maxHealth;
        
        public float StartingMaxHealth { get; }
        public IReadOnlyReactiveProperty<float> MaxHealth => _maxHealth.ReactiveValue;
        
        public HealthRestoreModel HealthRestoreModel { get; }
        public PlayerHealthModel(float maxHealth, IModifiableParameterOwner parameterOwner, HealthRestoreConfig healthRestoreConfig)
        {
            StartingMaxHealth = maxHealth;
            _maxHealth = new FloatModifiableParameter(Parameters.HEALTH, maxHealth, parameterOwner);
            HealthRestoreModel = new HealthRestoreModel(healthRestoreConfig);
        }

    }
}