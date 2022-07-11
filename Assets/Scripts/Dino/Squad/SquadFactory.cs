using Dino.App.Config;
using Dino.Config;
using Dino.Extension;
using Dino.Location;
using Dino.Location.Service;
using Dino.Modifiers.Config;
using Dino.Player.Inventory.Service;
using Dino.Squad.Config;
using Dino.Squad.Model;
using Dino.Units.Player.Config;
using Dino.Units.Service;
using Feofun.Config;
using Feofun.Modifiers;
using Zenject;

namespace Dino.Squad
{
    public class SquadFactory
    {
        private const string SQUAD_NAME = "Squad";

        [Inject] private World _world;
        [Inject] private WorldObjectFactory _worldObjectFactory;
        [Inject] private StringKeyedConfigCollection<PlayerUnitConfig> _playerUnitConfigs;
        [Inject] private SquadConfig _squadConfig;
        [Inject] private ConstantsConfig _constantsConfig;

        [Inject(Id = Configs.META_UPGRADES)]
        private StringKeyedConfigCollection<ParameterUpgradeConfig> _modifierConfigs;
        [Inject] private InventoryService _inventoryService;
        [Inject] private ModifierFactory _modifierFactory;

        public Squad CreateSquad()
        {
            var squad = _worldObjectFactory.CreateObject(SQUAD_NAME, _world.transform).RequireComponent<Squad>();
            squad.transform.SetPositionAndRotation(_world.Spawn.transform.position, _world.Spawn.transform.rotation);
            squad.Init(BuildSquadModel());
            return squad;
        }

        private SquadModel BuildSquadModel()
        {
            var startingHealth = _playerUnitConfigs.Get(_constantsConfig.FirstUnit).Health;
            return new SquadModel(_squadConfig, startingHealth, CreateParameterCalculator());
        }

        private MetaParameterCalculator CreateParameterCalculator() =>
                new MetaParameterCalculator(_inventoryService.Inventory.UnitsUpgrades, _modifierConfigs, _modifierFactory);
    }
}