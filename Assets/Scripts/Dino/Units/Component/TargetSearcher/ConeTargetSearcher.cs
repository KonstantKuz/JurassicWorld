using Dino.Extension;
using Dino.Units.Component.Target;
using Dino.Units.Enemy.Model;
using Dino.Weapon.Projectiles;
using Feofun.Components;
using UnityEngine;
using UnityEngine.Assertions;

namespace Dino.Units.Component.TargetSearcher
{
    public class ConeTargetSearcher : MonoBehaviour, IInitializable<Unit>, ITargetSearcher
    {
        private PatrolStateModel _stateModel;

        public void Init(Unit owner)
        {
            var enemyModel = (EnemyUnitModel) owner.Model;
            Assert.IsTrue(enemyModel != null, "Unit model must be EnemyUnitModel.");
            _stateModel = enemyModel.PatrolStateModel;
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
            return IsInsideCone(target.transform.position, transform.position, transform.forward, _stateModel.FieldOfViewAngle) && 
                IsInsideDistanceRange(target.transform.position, transform.position, 0, _stateModel.FieldOfViewDistance);
        }

        private static bool IsInsideCone(Vector3 target, Vector3 coneOrigin, Vector3 coneDirection, float maxAngle)
        {
            var targetDirection = target - coneOrigin;
            var angle = Vector3.Angle(coneDirection, targetDirection.XZ());
            return angle <= maxAngle;
        }

        private static bool IsInsideDistanceRange(Vector3 target, Vector3 origin, float distanceMin, float distanceMax)
        {
            var distance = Vector3.Distance(origin, target);
            return distance > distanceMin && distance < distanceMax;
        }
    }
}
