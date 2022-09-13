using System;
using System.Collections.Generic;
using System.Linq;
using Dino.Analytics;
using Dino.Location.Level.Service;
using Dino.Player.Progress.Service;
using Dino.Session.Service;
using Zenject;

namespace Dino.Core
{
    public class AnalyticsEventParamProvider: IEventParamProvider
    {
        [Inject] private LevelService _levelService;
        [Inject] private SessionService _sessionService;
        [Inject] private PlayerProgressService _playerProgressService;
        [Inject] private Feofun.ABTest.ABTest _abTest;

        public Dictionary<string, object> GetParams(IEnumerable<string> paramNames)
        {
            return paramNames.ToDictionary(it => it, GetValue);
        }

        private object GetValue(string paramName)
        {

            var playerProgress = _playerProgressService.Progress;

            return paramName switch
            {
                EventParams.LEVEL_ID => _sessionService.Session.LevelId,
                EventParams.LEVEL_NUMBER => GetLevelNumber(),
                EventParams.PASS_NUMBER => GetPassNumber(),
                EventParams.ENEMY_KILLED => _sessionService.Kills.Value,
                EventParams.TIME_SINCE_LEVEL_START => _sessionService.SessionTime,
                EventParams.WINS => playerProgress.WinCount,
                EventParams.DEFEATS => playerProgress.LoseCount,
                EventParams.CRAFT_COUNT => playerProgress.CraftCount,
                EventParams.LOOT_COUNT => playerProgress.LootCount,
                EventParams.AB_TEST_ID => _abTest.CurrentVariantId,

                _ => throw new ArgumentOutOfRangeException(nameof(paramName), paramName, $"Unsupported analytics parameter {paramName}")
            };
        }

        private int GetLevelNumber() 
        {
            var playerProgress = _playerProgressService.Progress;
            return playerProgress.LevelNumber + 1;
        }
        
        private int GetPassNumber()
        {
            var playerProgress = _playerProgressService.Progress;
            var level = _levelService.GetLevelById(_levelService.CurrentLevelId);
            return playerProgress.GetPassCount(level.ObjectId);
        }
    }
}
