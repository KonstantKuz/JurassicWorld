using System;
using System.Linq;
using Dino.Extension;
using Dino.Units.Component;
using Dino.Units.Enemy.Model;
using Feofun.Components;
using Feofun.Extension;
using UnityEngine;
using UnityEngine.Assertions;

namespace Dino.Units.Enemy
{
    public class EnemyFovRenderer : MonoBehaviour, IInitializable<Unit>, IUnitDeathEventReceiver
    {
        private IFieldOfViewRenderer _fovRenderer;

        private void Awake()
        {
            _fovRenderer = gameObject.RequireComponentInChildren<IFieldOfViewRenderer>();
        }

        public void Init(Unit unit)
        {
            var enemyModel = (EnemyUnitModel) unit.Model;
            Assert.IsTrue(enemyModel != null, "Unit model must be EnemyUnitModel.");
            var stateModel = enemyModel.PatrolStateModel;

            _fovRenderer.Init(stateModel.FieldOfViewAngle, stateModel.FieldOfViewDistance);
        }

        public void OnDeath(DeathCause deathCause)
        {
            var fovRenderer = (MonoBehaviour) _fovRenderer;
            fovRenderer.gameObject.SetActive(false);
        }
    }
}
