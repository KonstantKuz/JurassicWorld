using Dino.Config;
using Dino.Modifiers.Config;
using Dino.Player.Inventory.Service;
using Dino.Units.Model;
using Dino.Units.Player.Config;
using Dino.Units.Player.Model;
using Feofun.Config;
using Feofun.Modifiers;
using JetBrains.Annotations;
using Zenject;

namespace Dino.Units.Service
{
    [PublicAPI]
    public class PlayerUnitModelBuilder
    {
        [Inject(Id = Configs.MODIFIERS)]
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