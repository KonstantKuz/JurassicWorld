﻿using System;
using DinoWorldSurvival.Units.Model;
using DinoWorldSurvival.Units.Target;
using UnityEngine;

namespace DinoWorldSurvival.Units
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