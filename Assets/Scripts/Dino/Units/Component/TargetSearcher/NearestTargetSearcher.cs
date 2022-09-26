using System;
using System.Collections.Generic;
using Dino.Extension;
using Dino.Units.Component.Target;
using Dino.Weapon.Model;
using Feofun.Components;
using Feofun.Extension;
using JetBrains.Annotations;
using Logger.Extension;
using UnityEngine;
using Zenject;

namespace Dino.Units.Component.TargetSearcher
{
    [RequireComponent(typeof(ITarget))]
    public class NearestTargetSearcher : MonoBehaviour, IInitializable<Unit>, IInitializable<IWeaponModel>, ITargetSearcher, IDisposable
    {
        [SerializeField] private LayerMask _obstacleMask;

        [Inject] private TargetService _targetService;
        
        private float? _searchDistance;
        private ITarget _selfTarget;
        private UnitType _targetType;


        public void Init(Unit unit)
        {
            _selfTarget = gameObject.RequireComponent<ITarget>();
            _targetType = _selfTarget.UnitType.GetTargetUnitType();
        }

        public void Init(IWeaponModel weaponModel)
        {
            _searchDistance = weaponModel.TargetSearchRadius;
        }

        [CanBeNull]
        public ITarget Find()
        {
            if (_searchDistance == null)
            {
                this.Logger().Warn("Searcher need search distance");
                return null;
            }
            
            var targets = _targetService.AllTargetsOfType(_targetType);
            return Find(targets, _selfTarget.Root.position, _searchDistance.Value, _obstacleMask);
        }

        [CanBeNull]
        private static ITarget Find(IEnumerable<ITarget> targets, Vector3 from, float searchDistance, LayerMask obstacleMask)
        {
            ITarget result = null;
            var minDistance = Mathf.Infinity;
            foreach (var target in targets)
            {
                if (!target.IsAlive) continue;
                var dist = Vector3.Distance(from, target.Root.position);
                if (dist >= minDistance || dist > searchDistance) continue;
                if (Physics.Linecast(from, target.Root.position, obstacleMask)) continue;
                minDistance = dist;
                result = target;
            }
            
            return result;
        }

        public void Dispose()
        {
            _searchDistance = null;
        }
    }
}