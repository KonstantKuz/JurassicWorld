﻿using Feofun.Config;
using Feofun.Config.Serializers;
using Feofun.Localization.Config;
using Survivors.App.Config;
using Survivors.Config;
using Survivors.Enemy.Spawn.Config;
using Survivors.Loot.Config;
using Survivors.Modifiers.Config;
using Survivors.Reward.Config;
using Survivors.Session.Config;
using Survivors.Session.Model;
using Survivors.Shop.Config;
using Survivors.Units.Enemy.Config;
using Survivors.Squad.Config;
using Survivors.Squad.Upgrade.Config;
using Survivors.Squad.UpgradeSelection.Config;
using Survivors.Units.Player.Config;
using Zenject;

namespace Survivors.App
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