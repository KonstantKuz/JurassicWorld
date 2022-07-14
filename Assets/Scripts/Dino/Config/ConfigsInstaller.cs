using Dino.Loot.Config;
using Dino.Modifiers.Config;
using Dino.Reward.Config;
using Dino.Session.Config;
using Dino.Session.Model;
using Dino.Units.Enemy.Config;
using Dino.Units.Player.Config;
using Dino.Weapon.Config;
using Feofun.Config;
using Feofun.Config.Serializers;
using Feofun.Localization.Config;
using Zenject;

namespace Dino.Config
{
    public class ConfigsInstaller
    {
        public static void Install(DiContainer container)
        {
            new ConfigLoader(container, new CsvConfigDeserializer())
                .RegisterSingle<LocalizationConfig>(Configs.LOCALIZATION)
                .RegisterStringKeyedCollection<PlayerUnitConfig>(Configs.PLAYER_UNIT)
                .RegisterStringKeyedCollection<EnemyUnitConfig>(Configs.ENEMY_UNIT)
                .RegisterStringKeyedCollection<WeaponConfig>(Configs.WEAPONS)
                .RegisterSingleObjectConfig<ConstantsConfig>(Configs.CONSTANTS)
                
           
                .RegisterStringKeyedCollection<DroppingLootConfig>(Configs.DROPPING_LOOT)
                .RegisterStringKeyedCollection<ParameterUpgradeConfig>(Configs.MODIFIERS, true)
                .RegisterStringKeyedCollection<LevelMissionConfig>(Configs.LEVEL_MISSION)
                .RegisterCollection<SessionResult, MissionRewardsConfig>(Configs.MISSION_REWARDS);
        }
    }
}
