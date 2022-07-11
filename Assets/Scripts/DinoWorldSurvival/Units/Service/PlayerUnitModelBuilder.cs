using DinoWorldSurvival.Config;
using DinoWorldSurvival.Modifiers.Config;
using DinoWorldSurvival.Player.Inventory.Service;
using DinoWorldSurvival.Units.Model;
using DinoWorldSurvival.Units.Player.Config;
using DinoWorldSurvival.Units.Player.Model;
using Feofun.Config;
using Feofun.Modifiers;
using JetBrains.Annotations;
using Zenject;

namespace DinoWorldSurvival.Units.Service
{
    [PublicAPI]
    public class PlayerUnitModelBuilder
    {
        [Inject(Id = Configs.META_UPGRADES)]
        private readonly StringKeyedConfigCollection<ParameterUpgradeConfig> _modifierConfigs;
        [Inject]
        private readonly StringKeyedConfigCollection<PlayerUnitConfig> _playerUnitConfigs;
        [Inject]
        private readonly InventoryService _inventoryService;
        [Inject]
        private readonly ModifierFactory _modifierFactory;
        public IUnitModel BuildUnit(string unitId)
        {
            return new PlayerUnitModel(_playerUnitConfigs.Get(unitId), 
                                       new MetaParameterCalculator(_inventoryService.Inventory.UnitsUpgrades, _modifierConfigs, _modifierFactory));
        }
    }
}