using System.Collections.Generic;
using System.Linq;
using Dino.Extension;
using Dino.Location;
using Dino.Units.Enemy;
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
        [Inject] private StringKeyedConfigCollection<EnemyUnitConfig> _enemyUnitConfigs;
        [Inject] private EnemyAttacksConfig _attacksConfig;

        public void InitEnemies(IEnumerable<Unit> units)
        {
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

                var model = new EnemyUnitModel(config, _attacksConfig, unit.GetLevel());
                unit.Init(model);
            }
        }
    }
}
