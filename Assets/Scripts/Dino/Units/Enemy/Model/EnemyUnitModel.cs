using Dino.Units.Enemy.Config;
using Dino.Units.Model;
using UnityEngine;
using UnityEngine.Assertions;

namespace Dino.Units.Enemy.Model
{
    public class EnemyUnitModel : IUnitModel
    {
        private EnemyUnitConfig _config;
        public string Id { get; }
        public float MoveSpeed { get; }
        public int Level { get; }
        public IHealthModel HealthModel { get; }
        public IWeapon Weapon { get; }

        public EnemyUnitModel(EnemyUnitConfig config, int level = 1)
        {
            Assert.IsTrue(level >= EnemyUnitConfig.MIN_LEVEL);
            _config = config;
            Id = config.Id;
            MoveSpeed = config.MoveSpeed;
            Level = level;
            HealthModel = new EnemyHealthModel(config.GetHealthForLevel(level));
            Weapon = new EnemyWeapon(config.EnemyAttackConfig);
        }
        public int CalculateLevelOfHealth(float currentHealth)
        {
            return currentHealth <= _config.Health ? EnemyUnitConfig.MIN_LEVEL : EnemyUnitConfig.MIN_LEVEL + (int) Mathf.Ceil((currentHealth - _config.Health) / _config.HealthStep);
        }
        public float CalculateScale(int level) => _config.CalculateScale(level);
    }
}