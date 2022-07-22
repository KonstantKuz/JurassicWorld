﻿using Dino.Units.Model;
using Dino.Units.Player.Config;
using Dino.Units.Service;
using Feofun.Modifiers;

namespace Dino.Units.Player.Model
{
    public class PlayerUnitModel : ModifiableParameterOwner, IUnitModel
    {
        private readonly PlayerUnitConfig _config;

        public PlayerUnitModel(PlayerUnitConfig config)
        {
            _config = config;
            HealthModel = new PlayerHealthModel(config.Health, this);
        }

        public string Id => _config.Id;
        public float MoveSpeed => _config.MoveSpeed;
        public IHealthModel HealthModel { get; }
    }
}