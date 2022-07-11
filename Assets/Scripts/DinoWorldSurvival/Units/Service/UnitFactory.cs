using DinoWorldSurvival.Extension;
using DinoWorldSurvival.Location;
using DinoWorldSurvival.Location.Service;
using DinoWorldSurvival.Units.Enemy.Config;
using DinoWorldSurvival.Units.Enemy.Model;
using Feofun.Config;
using ModestTree;
using Zenject;

namespace DinoWorldSurvival.Units.Service
{
    public class UnitFactory
    {
        [Inject] private World _world;
        [Inject] private WorldObjectFactory _worldObjectFactory;
        [Inject] private StringKeyedConfigCollection<EnemyUnitConfig> _enemyUnitConfigs;
        [Inject] private PlayerUnitModelBuilder _playerUnitModelBuilder;
        
        public void CreatePlayerUnits(string unitId, int count)
        {
            for (int i = 0; i < count; i++) {
                CreatePlayerUnit(unitId);
            }
        }

        private void CheckSquad() => Assert.IsNotNull(_world.Squad, "Squad is null, should call this method only inside game session");

        public Unit CreatePlayerUnit(string unitId)
        {
            CheckSquad();
            var unit = _worldObjectFactory.CreateObject(unitId).RequireComponent<Unit>();
            var model = _playerUnitModelBuilder.BuildUnit(unitId);
            unit.Init(model);
            _world.Squad.AddUnit(unit);
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