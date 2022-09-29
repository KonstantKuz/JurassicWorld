using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Dino.Units.Enemy.Config
{
    public class EnemyAttackConfigs
    {
        private const string CONFIGS_PATH = "Configs/AttackConfigs/";

        private List<EnemyAttackConfig> _attackConfigs;
        private List<EnemyAttackConfig> AttackConfigs => _attackConfigs ??= LoadConfigs();
        
        public T Get<T>(AttackVariant variant) where T : EnemyAttackConfig
        {
            return AttackConfigs.First(it => it.AttackVariant == variant) as T;
        }

        public EnemyAttackConfig Get(AttackVariant variant)
        {
            return AttackConfigs.First(it => it.AttackVariant == variant);
        }

        private List<EnemyAttackConfig> LoadConfigs() => Resources.LoadAll<ScriptableObject>(CONFIGS_PATH).Select(it => it as EnemyAttackConfig).ToList();
    }
}