using Dino.Extension;
using Dino.Units.Enemy.Model;
using Dino.Units.Weapon;
using Feofun.Components;
using UnityEngine;
using UnityEngine.Assertions;

namespace Dino.Units.Enemy
{
    public class EnemyFovRenderer : MonoBehaviour, IInitializable<IUnit>
    {
        public void Init(IUnit unit)
        {
            var owner = (Unit) unit;
            var enemyModel = (EnemyUnitModel) owner.Model;
            Assert.IsTrue(enemyModel != null, "Unit model must be EnemyUnitModel.");
            var stateModel = enemyModel.PatrolStateModel;
                
            var coneRenderer = owner.GameObject.RequireComponentInChildren<RangeConeRenderer>();
            coneRenderer.Build(stateModel.FieldOfViewAngle, stateModel.FieldOfViewDistance);
        }
    }
}
