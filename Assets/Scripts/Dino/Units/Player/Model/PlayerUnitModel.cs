using Dino.Units.Model;
using Dino.Units.Player.Config;
using Dino.Units.Service;
using Feofun.Modifiers;

namespace Dino.Units.Player.Model
{
    public class PlayerUnitModel : ModifiableParameterOwner, IUnitModel
    {
        private readonly PlayerUnitConfig _config;
        private readonly PlayerAttackModel _playerAttackModel;

        public PlayerUnitModel(PlayerUnitConfig config, MetaParameterCalculator parameterCalculator)
        {
            _config = config;
            HealthModel = new PlayerHealthModel(config.Health, this);
            _playerAttackModel = new PlayerAttackModel(config.PlayerAttackConfig, this, parameterCalculator);
        }

        public string Id => _config.Id;
        public float MoveSpeed => _config.MoveSpeed;
        public IHealthModel HealthModel { get; }
        public IAttackModel AttackModel => _playerAttackModel;
    }
}