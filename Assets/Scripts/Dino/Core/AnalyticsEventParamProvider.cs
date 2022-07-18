using System;
using System.Collections.Generic;
using System.Linq;
using Dino.Analytics;
using Dino.Location.Service;
using Dino.Player.Progress.Service;
using Dino.Session.Service;
using UnityEngine;
using Zenject;

namespace Dino.Core
{
    public class AnalyticsEventParamProvider: IEventParamProvider
    {
        [Inject] private LevelService _levelService;
        [Inject] private SessionService _sessionService;
        [Inject] private PlayerProgressService _playerProgressService;

        
        public Dictionary<string, object> GetParams(IEnumerable<string> paramNames)
        {
            return paramNames.ToDictionary(it => it, GetValue);
        }

        private object GetValue(string paramName)
        {

            var playerProgress = _playerProgressService.Progress;
            
            return paramName switch
            {
                EventParams.LEVEL_ID => _sessionService.CurrentLevelId,
                EventParams.LEVEL_NUMBER => GetLevelNumber(),
                EventParams.LEVEL_LOOP => GetLevelLoop(),

                EventParams.ENEMY_KILLED => _sessionService.Kills.Value,
                EventParams.TIME_SINCE_LEVEL_START => _sessionService.SessionTime,
                EventParams.PASS_NUMBER => GetPassNumber(),
                EventParams.STAND_RATIO => GetStandRatio(),
                EventParams.TOTAL_KILLS => playerProgress.Kills,
                EventParams.WINS => playerProgress.WinCount,
                EventParams.DEFEATS => playerProgress.LoseCount,
                EventParams.REVIVE_COUNT => _sessionService.Session.Revives,
                
                _ => throw new ArgumentOutOfRangeException(nameof(paramName), paramName, $"Unsupported analytics parameter {paramName}")
            };
        }

        private int GetLevelNumber() 
        {
            var playerProgress = _playerProgressService.Progress;
            return playerProgress.LevelNumber + 1;
        }

        private int GetLevelLoop()
        {
            var playerProgress = _playerProgressService.Progress;
            return Mathf.Max(0, playerProgress.LevelNumber - _levelService.Levels.Count);
        }
        

        private int GetPassNumber()
        {
            var playerProgress = _playerProgressService.Progress;
            return playerProgress.GetPassCount(_sessionService.CurrentLevelId);
        }

        private float GetStandRatio()
        {
            throw new NotImplementedException();
        }
    }
}
