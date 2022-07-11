﻿using Dino.Modifiers.Config;
using Dino.Player.Inventory.Model;
using Feofun.Config;
using Feofun.Modifiers;
using Feofun.Modifiers.Parameters;

namespace Dino.Units.Service
{
    public class MetaParameterCalculator
    {
        private readonly UnitsMetaUpgrades _unitsUpgrades;
        private readonly StringKeyedConfigCollection<ParameterUpgradeConfig> _modifierConfigs;
        private readonly ModifierFactory _modifierFactory;

        public MetaParameterCalculator(UnitsMetaUpgrades unitsUpgrades,
                                       StringKeyedConfigCollection<ParameterUpgradeConfig> modifierConfigs,
                                       ModifierFactory modifierFactory)
        {
            _unitsUpgrades = unitsUpgrades;
            _modifierConfigs = modifierConfigs;
            _modifierFactory = modifierFactory;
        }
        public void InitParam(FloatModifiableParameter parameter, IModifiableParameterOwner owner)
        {
            parameter.Reset();
            var upgradeCount = _unitsUpgrades.GetUpgradeLevel(parameter.Name);
            if (upgradeCount <= 0) return;
            var modificatorConfig = _modifierConfigs.Find(parameter.Name);
            if (modificatorConfig == null) return;
            var modificator = _modifierFactory.Create(modificatorConfig.ModifierConfig);
            for (int i = 0; i < upgradeCount; i++) {
                modificator.Apply(owner);
            }
        }
    }
}