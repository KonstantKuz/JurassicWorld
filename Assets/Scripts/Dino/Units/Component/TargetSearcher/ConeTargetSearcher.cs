using System.Collections.Generic;
using Dino.Extension;
using Dino.Units.Enemy.Model;
using Dino.Units.Target;
using Dino.Units.Weapon;
using Dino.Units.Weapon.Projectiles;
using Feofun.Components;
using UnityEngine;
using UnityEngine.Assertions;

namespace Dino.Units.Component.TargetSearcher
{
    public class ConeTargetSearcher : MonoBehaviour, IInitializable<IUnit>, ITargetSearcher, IUpdatableComponent
    {
        private ITargetProvider _targetProvider;
        private PatrolStateModel _stateModel;

        public void Init(IUnit owner)
        {
            var enemyModel = (EnemyUnitModel) owner.Model;
            Assert.IsTrue(enemyModel != null, "Unit model must be EnemyUnitModel.");
            _stateModel = enemyModel.PatrolStateModel;

            _targetProvider = owner.GameObject.RequireComponent<ITargetProvider>();
            var coneRenderer = owner.GameObject.RequireComponentInChildren<RangeConeRenderer>();
            coneRenderer.Build(_stateModel.FieldOfViewAngle, _stateModel.FieldOfViewDistance);
        }

        public void OnTick()
        {
            if (_targetProvider.Target == null || !_targetProvider.Target.IsTargetValidAndAlive())
            {
                _targetProvider.Target = Find();
            }
        }

        public ITarget Find()
        {
            var hits = Projectile.GetHits(transform.position, _stateModel.FieldOfViewDistance, UnitType.PLAYER);
            foreach (var hit in hits)
            {
                if (IsInsideFieldOfView(hit) && Projectile.CanDamageTarget(hit, UnitType.PLAYER, out var target))
                {
                    return target;
                }
            }

            return null;
        }

        private bool IsInsideFieldOfView(Collider target)
        {
            return FlameCharge.IsInsideCone(target.transform.position, transform.position, transform.forward, _stateModel.FieldOfViewAngle) && 
                FlameCharge.IsInsideDistanceRange(target.transform.position, transform.position, 0, _stateModel.FieldOfViewDistance);
        }

        public IEnumerable<ITarget> GetAllOrderedByDistance()
        {
            throw new System.NotImplementedException();
        }
    }
}
