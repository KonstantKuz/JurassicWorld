using System.Collections.Generic;
using System.Linq;
using DinoWorldSurvival.Units.Target;
using Feofun.Components;
using UnityEngine;
using Zenject;

namespace DinoWorldSurvival.Units.Component.TargetSearcher
{
    public class OptimizedTargetProvider : MonoBehaviour, IInitializable<Unit>
    {
        private struct TargetRecord
        {
            public ITarget Target;
            public float DistanceToSquad;
        }

        private const int SEARCH_COUNT_PER_UNIT = 20;
        private static UnitType TargetType => UnitType.ENEMY; 

        private Unit _owner;
        private List<TargetRecord> _targets = new List<TargetRecord>();

        [Inject] private TargetService _targetService;

        public IEnumerable<ITarget> Targets => _targets.Select(it => it.Target);

        public void Init(Unit owner)
        {
            _owner = owner;
        }

        public ITarget GetTargetBy(Vector3 position, float searchDistance)
        {
            var targets = _targets.Take(SEARCH_COUNT_PER_UNIT).Select(it => it.Target);
            return NearestTargetSearcher.Find(targets, position, searchDistance);
        }

        private void Update()
        {
            if (_owner == null)
            {
                return;
            }
            
            var ownerPosition = _owner.transform.position;
            _targets = _targetService.AllTargetsOfType(TargetType).Select(it => 
                new TargetRecord
                {
                    Target = it,
                    DistanceToSquad = Vector3.Distance(ownerPosition, it.Root.position)
                }).OrderBy(it => it.DistanceToSquad)
                .ToList();
        }
    }
}