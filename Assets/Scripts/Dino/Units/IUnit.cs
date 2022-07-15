using System;
using Dino.Units.Component.Target;
using Dino.Units.Model;
using UnityEngine;

namespace Dino.Units
{
    public interface IUnit
    {
        UnitType UnitType { get; }
        IUnitModel Model { get; }
        ITarget SelfTarget { get; }
        GameObject GameObject { get; }
        public bool IsActive { get; set; }
        event Action<IUnit, DeathCause> OnDeath;
        event Action<IUnit> OnUnitDestroyed;
        public void Init(IUnitModel model);
        public void Kill(DeathCause deathCause);
    }
}