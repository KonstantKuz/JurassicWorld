using Dino.Units.Enemy.Model;
using UnityEngine.Assertions;

namespace Dino.Units
{
    public static class UnitExtension
    {
        public static EnemyUnitModel RequireEnemyModel(this Unit unit)
        {
            var enemyModel = (EnemyUnitModel) unit.Model;
            Assert.IsTrue(enemyModel != null, "Unit model must be EnemyUnitModel.");
            return enemyModel;
        }
    }
}