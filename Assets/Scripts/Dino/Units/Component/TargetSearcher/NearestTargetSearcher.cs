﻿using System.Collections.Generic;
using System.Linq;
using Dino.Extension;
using Dino.Units.Model;
using Dino.Units.Target;
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

        private IWeapon _weapon;    
        private ITarget _selfTarget;
        private UnitType _targetType;

        private float SearchDistance => _weapon.TargetSearchRadius;

        public void Init(IUnit unit)
        {
            _weapon = unit.Model.AttackModel;
            _selfTarget = gameObject.RequireComponent<ITarget>();
            _targetType = _selfTarget.UnitType.GetTargetUnitType();
        }

        [CanBeNull]
        public ITarget Find()
        {
            var targets = _targetService.AllTargetsOfType(_targetType);
            return Find(targets, _selfTarget.Root.position, SearchDistance);
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

        public IEnumerable<ITarget> GetAllOrderedByDistance()
        {
            return _targetService.AllTargetsOfType(_targetType)
                .Where(IsDistanceReached)
                .OrderBy(it => Vector3.Distance(it.Root.position, _selfTarget.Root.position));
        }

        private bool IsDistanceReached(ITarget target) => 
            Vector3.Distance(target.Root.position, _selfTarget.Root.position) <= SearchDistance;
    }
}