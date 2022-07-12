using System.Collections.Generic;
using Dino.Extension;
using Dino.Units.Target;
using Feofun.Components;
using JetBrains.Annotations;
using UnityEngine;

namespace Dino.Units.Component.TargetSearcher
{
    public class OptimizedTargetSearcher : MonoBehaviour, ITargetSearcher, IInitializable<IUnit>
    {
        private IUnit _owner;
        private OptimizedTargetProvider _targetProvider;
        public void Init(IUnit owner)
        {
            _owner = owner;
            _targetProvider = _owner.GameObject.RequireComponent<OptimizedTargetProvider>();
        }

        [CanBeNull]
        public ITarget Find()
        {
            var position = _owner.SelfTarget.Root.position;
            var searchDistance = _owner.Model.AttackModel.TargetSearchRadius;
            return _targetProvider.GetTargetBy(position, searchDistance);
        }
        
        public IEnumerable<ITarget> GetAllOrderedByDistance()
        {
            return _targetProvider.Targets;
        }
    }
}