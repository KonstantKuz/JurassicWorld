using Dino.Location;
using Dino.Units.Enemy.Config;
using Dino.Units.Enemy.Model;
using Feofun.Config;
using Logger.Extension;
using UnityEngine;
using Zenject;

namespace Dino.Units.Service
{
    public class EnemyInitService : MonoBehaviour
    {
        [Inject] private World _world;
        [Inject] private StringKeyedConfigCollection<EnemyUnitConfig> _enemyUnitConfigs;
        
        public void InitEnemies()
        {
            var units = _world.GetChildrenComponents<Unit>();
            foreach (var unit in units)
            {
                if (!unit.gameObject.activeSelf)
                {
                    continue;
                }
                var config = _enemyUnitConfigs.Find(unit.ObjectId);
                if (config == null)
                {
                    this.Logger().Warn($"There is no suitable config for {unit.ObjectId}");
                    continue;
                }
                var model = new EnemyUnitModel(config);
                unit.Init(model);
            }
        }
    }
}
