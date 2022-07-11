﻿using DinoWorldSurvival.Modifiers;
using DinoWorldSurvival.Squad.Config;
using DinoWorldSurvival.Units.Model;
using DinoWorldSurvival.Units.Service;
using Feofun.Modifiers;
using Feofun.Modifiers.Modifiers;
using Feofun.Modifiers.Parameters;
using UniRx;

namespace DinoWorldSurvival.Squad.Model
{
    public class SquadModel : ModifiableParameterOwner
    {
        private readonly FloatModifiableParameter _speed;
        private readonly FloatModifiableParameter _collectRadius;
        private readonly FloatModifiableParameter _startingUnitModifiableCount;
        private readonly IReadOnlyReactiveProperty<int> _startingUnitCount;

        public SquadModel(SquadConfig config, float startingHealth, MetaParameterCalculator parameterCalculator)
        {
            _speed = new FloatModifiableParameter(Parameters.SPEED, config.Speed, this);
            _collectRadius = new FloatModifiableParameter(Parameters.COLLECT_RADIUS, config.CollectRadius, this);

            _startingUnitModifiableCount = new FloatModifiableParameter(Parameters.STARTING_UNIT_COUNT, 1, this);
            parameterCalculator.InitParam(_startingUnitModifiableCount, this);
            _startingUnitCount = _startingUnitModifiableCount.ReactiveValue.Select(it => (int) it).ToReactiveProperty();

            HealthModel = new SquadHealthModel(this, startingHealth, parameterCalculator);
        }

        public void AddUnit(IUnitModel unitModel)
        {
            var addHealthModifier = new AddValueModifier(Parameters.HEALTH, unitModel.HealthModel.MaxHealth.Value);
            AddModifier(addHealthModifier);
        }

        public IHealthModel HealthModel { get; }

        public IReadOnlyReactiveProperty<int> StartingUnitCount => _startingUnitCount;
        public IReadOnlyReactiveProperty<float> Speed => _speed.ReactiveValue;
        public IReadOnlyReactiveProperty<float> CollectRadius => _collectRadius.ReactiveValue;
    }
}