using DinoWorldSurvival.Modifiers;
using DinoWorldSurvival.Units.Model;
using DinoWorldSurvival.Units.Service;
using Feofun.Modifiers;
using Feofun.Modifiers.Parameters;
using UniRx;

namespace DinoWorldSurvival.Squad.Model
{
    public class SquadHealthModel : IHealthModel
    {
        private readonly FloatModifiableParameter _maxHealth;

        public SquadHealthModel(IModifiableParameterOwner parameterOwner, float startingHealth, MetaParameterCalculator parameterCalculator)
        {
            StartingMaxHealth = startingHealth;
            
            _maxHealth = new FloatModifiableParameter(Parameters.HEALTH, 0, parameterOwner);  
            parameterCalculator.InitParam(_maxHealth, parameterOwner);
        }
        public float StartingMaxHealth { get; }
        public IReadOnlyReactiveProperty<float> MaxHealth => _maxHealth.ReactiveValue;
    }
}