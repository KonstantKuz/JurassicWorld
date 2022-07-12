using Dino.Extension;
using Dino.Location.Service;
using Dino.Units.Enemy.Config;
using Dino.Units.Enemy.Model;
using Feofun.Config;
using Zenject;

namespace Dino.Units.Service
{
    public class UnitFactory
    {
        [Inject] private WorldObjectFactory _worldObjectFactory;
        [Inject] private StringKeyedConfigCollection<EnemyUnitConfig> _enemyUnitConfigs;
        [Inject] private PlayerUnitModelBuilder _playerUnitModelBuilder;
        
        public Unit CreatePlayerUnit(string unitId)
        {
            var unit = _worldObjectFactory.CreateObject(unitId).RequireComponent<Unit>();
            var model = _playerUnitModelBuilder.BuildUnit(unitId);
            unit.Init(model);
            return unit;
        }
        
        public Unit CreateEnemy(string unitId, int level)
        {
            var enemy = _worldObjectFactory.CreateObject(unitId).RequireComponent<Unit>();
            var config = _enemyUnitConfigs.Get(unitId);
            var model = new EnemyUnitModel(config, level);
            enemy.Init(model);
            return enemy;
        }
    }
}