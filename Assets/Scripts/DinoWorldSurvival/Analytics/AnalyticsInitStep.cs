﻿using Feofun.App.Init;
using Zenject;

namespace DinoWorldSurvival.Analytics
{
    public class AnalyticsInitStep: AppInitStep
    {
        [Inject] private Analytics _analytics;
        protected override void Run()
        {
            _analytics.Init();
            Next();
        }
    }
}