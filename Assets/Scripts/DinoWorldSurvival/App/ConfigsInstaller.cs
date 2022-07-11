using DinoWorldSurvival.App.Config;
using DinoWorldSurvival.Config;
using DinoWorldSurvival.Enemy.Spawn.Config;
using DinoWorldSurvival.Loot.Config;
using DinoWorldSurvival.Modifiers.Config;
using DinoWorldSurvival.Reward.Config;
using DinoWorldSurvival.Session.Config;
using DinoWorldSurvival.Session.Model;
using DinoWorldSurvival.Shop.Config;
using DinoWorldSurvival.Squad.Config;
using DinoWorldSurvival.Squad.Upgrade.Config;
using DinoWorldSurvival.Squad.UpgradeSelection.Config;
using DinoWorldSurvival.Units.Enemy.Config;
using DinoWorldSurvival.Units.Player.Config;
using Feofun.Config;
using Feofun.Config.Serializers;
using Feofun.Localization.Config;
using Zenject;

namespace DinoWorldSurvival.App
{
    public class ConfigsInstaller
    {
        public static void Install(DiContainer container)
        {
            new ConfigLoader(container, new CsvConfigDeserializer())
                .RegisterSingle<LocalizationConfig>(Configs.LOCALIZATION)
                .RegisterSingle<EnemyWavesConfig>(Configs.ENEMY_WAVES)
                .RegisterStringKeyedCollection<PlayerUnitConfig>(Configs.PLAYER_UNIT)
                .RegisterStringKeyedCollection<EnemyUnitConfig>(Configs.ENEMY_UNIT)
                .RegisterStringKeyedCollection<DroppingLootConfig>(Configs.DROPPING_LOOT)
                .RegisterStringKeyedCollection<SquadLevelConfig>(Configs.SQUAD_LEVEL)
                .RegisterSingleObjectConfig<SquadConfig>(Configs.SQUAD)
                .RegisterStringKeyedCollection<ParameterUpgradeConfig>(Configs.MODIFIERS, true)            
                .RegisterStringKeyedCollection<UpgradeProductConfig>(Configs.META_UPGRADES_SHOP)
                .RegisterSingle<UpgradesConfig>(Configs.UPGRADES)          
                .RegisterStringKeyedCollection<ParameterUpgradeConfig>(Configs.META_UPGRADES, true)  
                .RegisterSingleObjectConfig<UpgradeBranchSelectionConfig>(Configs.CONSTANTS)  
                .RegisterSingleObjectConfig<ConstantsConfig>(Configs.CONSTANTS)
                .RegisterSingleObjectConfig<HpsSpawnerConfig>(Configs.ENEMY_SPAWNER)
                .RegisterStringKeyedCollection<LevelMissionConfig>(Configs.LEVEL_MISSION)
                .RegisterCollection<SessionResult, MissionRewardsConfig>(Configs.MISSION_REWARDS);
        }
    }
}