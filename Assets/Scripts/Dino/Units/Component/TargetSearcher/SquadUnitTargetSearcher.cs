using System.Collections.Generic;
using Dino.Squad.Component;
using Dino.Units.Target;
using Feofun.Components;
using JetBrains.Annotations;
using UnityEngine;

namespace Dino.Units.Component.TargetSearcher
{
    public class SquadUnitTargetSearcher : MonoBehaviour, ITargetSearcher, IInitializable<IUnit>, IInitializable<Squad.Squad>
    {
        private IUnit _owner;
        private SquadTargetProvider _targetProvider;
        public void Init(IUnit owner)
        {
            _owner = owner;
        }

        public void Init(Squad.Squad owner)
        {
            _targetProvider = owner.TargetProvider;
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