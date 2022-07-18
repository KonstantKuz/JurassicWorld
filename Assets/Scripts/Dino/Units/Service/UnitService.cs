using System;
using System.Collections.Generic;
using System.Linq;
using Dino.Units.Messages;
using JetBrains.Annotations;
using SuperMaxim.Core.Extensions;
using SuperMaxim.Messaging;
using UnityEngine;
using Zenject;

namespace Dino.Units.Service
{
    [PublicAPI]
    public class UnitService
    {
        private readonly Dictionary<UnitType, HashSet<Unit>> _units = new Dictionary<UnitType, HashSet<Unit>>();
        public event Action<Unit> OnPlayerUnitDeath;
        public event Action<Unit, DeathCause> OnEnemyUnitDeath;

        public IEnumerable<Unit> AllUnits => _units.SelectMany(it => it.Value);

        [Inject] private IMessenger _messenger;
        
        public void Add(Unit unit)
        {
            if (!_units.ContainsKey(unit.UnitType)) {
                _units[unit.UnitType] = new HashSet<Unit>();
            }
            _units[unit.UnitType].Add(unit);
            unit.OnDeath += OnDeathUnit;

            _messenger.Publish(new UnitSpawnedMessage(unit));
        }
        public void Remove(Unit unit)
        {
            _units[unit.UnitType].Remove(unit);
            unit.OnDeath -= OnDeathUnit;
        }
        public void DeactivateAll() => AllUnits.ForEach(u => { u.IsActive = false; });
        public bool HasUnitOfType(UnitType unitType) => _units.ContainsKey(unitType) && _units[unitType].Any();

        private void OnDeathUnit(Unit unit, DeathCause deathCause)
        {
            unit.OnDeath -= OnDeathUnit;
            Remove(unit);
            if (unit.UnitType == UnitType.PLAYER) {
                OnPlayerUnitDeath?.Invoke(unit);
            } else {
                OnEnemyUnitDeath?.Invoke(unit, deathCause);
            }
        }

        public IEnumerable<Unit> GetAllUnitsOfType(UnitType unitType) =>
            _units.ContainsKey(unitType) ? _units[unitType] : Enumerable.Empty<Unit>();
        
        public IEnumerable<Unit> GetEnemyUnits()
        {
            return GetAllUnits(UnitType.ENEMY);
        }

        public IEnumerable<Unit> GetAllUnits(UnitType unitType)
        {
            return GetAllUnitsOfType(unitType)
                .Select(it => it as Unit)
                .Where(it => it != null);
        }
        
        public IEnumerable<Unit> GetUnitsInRadius(Vector3 from, UnitType unitType, float radius)
        {
            foreach (var target in GetAllUnits(unitType))
            {
                var distance = Vector3.Distance(from, target.SelfTarget.Root.position);
                if(distance > radius)
                    continue;
                yield return target;
            }
        }
    }
}