using System.Linq;
using Dino.Extension;
using Dino.Units.Component;
using Dino.Units.Enemy.Model;
using Feofun.Components;
using UnityEngine;
using UnityEngine.Assertions;

namespace Dino.Units.Enemy
{
    public class EnemyFovRenderer : MonoBehaviour, IInitializable<Unit>, IUnitDeathEventReceiver
    {
        public void Init(Unit unit)
        {
            var enemyModel = (EnemyUnitModel) unit.Model;
            Assert.IsTrue(enemyModel != null, "Unit model must be EnemyUnitModel.");
            var stateModel = enemyModel.PatrolStateModel;

            var fovRenderer = gameObject.RequireComponentInChildren<IFieldOfViewRenderer>();
            fovRenderer.Init(stateModel.FieldOfViewAngle, stateModel.FieldOfViewDistance);
        }

        public void OnDeath(DeathCause deathCause)
        {
            var fovRenderer = (MonoBehaviour) gameObject.RequireComponentInChildren<IFieldOfViewRenderer>();
            fovRenderer.gameObject.SetActive(false);
        }
    }
}
