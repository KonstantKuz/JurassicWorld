using System.Linq;
using Dino.Extension;
using Dino.Units.Component;
using Dino.Units.Enemy.Model;
using Feofun.Components;
using UnityEngine;
using UnityEngine.Assertions;

namespace Dino.Units.Enemy
{
    public class EnemyFovRenderer : MonoBehaviour, IInitializable<Unit>
    {
        public void Init(Unit unit)
        {
            var enemyModel = (EnemyUnitModel) unit.Model;
            Assert.IsTrue(enemyModel != null, "Unit model must be EnemyUnitModel.");
            var stateModel = enemyModel.PatrolStateModel;

            var renderers = unit.GameObject.GetComponentsInChildren<Transform>(true).Where(it => it.gameObject.GetComponent<IFieldOfViewRenderer>() != null);
            var activeRenderer = renderers.First(it => it.gameObject.activeSelf);
            var coneRenderer = activeRenderer.gameObject.RequireComponent<IFieldOfViewRenderer>();
            coneRenderer.Init(stateModel.FieldOfViewAngle, stateModel.FieldOfViewDistance);
        }
    }
}
