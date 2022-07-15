using Dino.Units.Model;
using Dino.Units.Player.Config;
using Dino.Units.Player.Model;
using Feofun.Config;
using JetBrains.Annotations;
using Zenject;

namespace Dino.Units.Service
{
    [PublicAPI]
    public class PlayerUnitModelBuilder
    {
        [Inject]
        private readonly StringKeyedConfigCollection<PlayerUnitConfig> _playerUnitConfigs;
        public IUnitModel BuildUnit(string unitId)
        {
            return new PlayerUnitModel(_playerUnitConfigs.Get(unitId));
        }
    }
}