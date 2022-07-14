using System.Collections.Generic;
using Dino.Extension;
using Dino.Units.Model;
using Dino.Units.Target;
using Dino.Weapon.Model;
using Feofun.Components;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace Dino.Units.Component.TargetSearcher
{
    [RequireComponent(typeof(ITarget))]
    public class NearestTargetSearcher : MonoBehaviour, IInitializable<IUnit>, ITargetSearcher
    {
        [Inject]
        private TargetService _targetService;

        private IWeaponModel _weaponModel;    
        private ITarget _selfTarget;
        private UnitType _targetType;
        

        public void Init(IUnit unit)
        {
            _selfTarget = gameObject.RequireComponent<ITarget>();
            _targetType = _selfTarget.UnitType.GetTargetUnitType();
        }

        [CanBeNull]
        public ITarget Find(float searchDistance)
        {
            var targets = _targetService.AllTargetsOfType(_targetType);
            return Find(targets, _selfTarget.Root.position, searchDistance);
        }

        [CanBeNull]
        public static ITarget Find(IEnumerable<ITarget> targets, Vector3 from, float searchDistance)
        {
            ITarget result = null;
            var minDistance = Mathf.Infinity;
            foreach (var target in targets)
            {
                if (!target.IsAlive) continue;
                var dist = Vector3.Distance(from, target.Root.position);
                if (dist >= minDistance || dist > searchDistance) continue;
                minDistance = dist;
                result = target;
            }
            
            return result;
        }
    }
}