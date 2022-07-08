﻿using Feofun.Components;
using Survivors.Units.Component.Health;

namespace Survivors.Squad.Component
{
    public class SquadWithHealth : Health, IInitializable<Squad>
    {
        public void Init(Squad squad)
        {
            base.Init(squad.Model.HealthModel);
        }
    }
}