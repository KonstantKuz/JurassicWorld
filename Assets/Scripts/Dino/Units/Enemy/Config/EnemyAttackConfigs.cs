using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Dino.Units.Enemy.Config
{
    public class EnemyAttackConfigs
    {
        private const string CONFIGS_PATH = "Configs/AttackConfigs/";

        public static EnemyAttackConfig Get(AttackVariant variant)
        {
            var configs = GetAll();
            return configs.First(it => it.AttackVariant == variant);
        }

        private static List<EnemyAttackConfig> GetAll() => Resources.LoadAll<ScriptableObject>(CONFIGS_PATH).Select(it => it as EnemyAttackConfig).ToList();
    }
}