﻿using Dino.Extension;
using Dino.Location.Service;
using Dino.Units.Enemy.Config;
using Dino.Units.Enemy.Model;
using Dino.Units.Player;
using Feofun.Config;
using UnityEngine;
using Zenject;

namespace Dino.Units.Service
{
    public class UnitFactory
    {
        [Inject] private WorldObjectFactory _worldObjectFactory;
        [Inject] private StringKeyedConfigCollection<EnemyUnitConfig> _enemyUnitConfigs;
        [Inject] private PlayerUnitModelBuilder _playerUnitModelBuilder;
        
        public PlayerUnit CreatePlayerUnit(string unitId, Vector3 position = new Vector3())
        {
            var unit = _worldObjectFactory.CreateObject(unitId).RequireComponent<PlayerUnit>();
            var model = _playerUnitModelBuilder.BuildUnit(unitId);
            unit.Init(model);
            unit.transform.position = position;
            return unit;
        }
        
        public Unit CreateEnemy(string unitId)
        {
            var enemy = _worldObjectFactory.CreateObject(unitId).RequireComponent<Unit>();
            var config = _enemyUnitConfigs.Get(unitId);
            var model = new EnemyUnitModel(config);
            enemy.Init(model);
            return enemy;
        }
    }
}