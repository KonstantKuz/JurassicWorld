﻿namespace DinoWorldSurvival.Units
{
    public interface IUnitDeathEventReceiver
    {
        void OnDeath(DeathCause deathCause);
    }
}