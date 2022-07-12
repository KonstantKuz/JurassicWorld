using Dino.Units.Enemy.Config;
using Dino.Units.Enemy.Model;
using Feofun.Config;
using UnityEngine;
using Zenject;

namespace Dino.Units.Service
{
    public class EnemyInitService : MonoBehaviour
    {
        [Inject] private StringKeyedConfigCollection<EnemyUnitConfig> _enemyUnitConfigs;
        
        public void InitEnemies()
        {
            var units = FindObjectsOfType<Unit>();
            foreach (var unit in units)
            {
                foreach (var id in _enemyUnitConfigs.Keys)
                {
                    if (!unit.name.Contains(id)) continue;
                    var model = new EnemyUnitModel(_enemyUnitConfigs.Get(id));
                    unit.Init(model);
                }
            }
        }
    }
}
