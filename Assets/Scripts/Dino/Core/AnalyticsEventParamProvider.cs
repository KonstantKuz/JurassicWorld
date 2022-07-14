using System;
using System.Collections.Generic;
using System.Linq;
using Dino.Analytics;
using Dino.Location;
using Dino.Player.Progress.Service;
using Dino.Session.Config;
using Dino.Session.Service;
using Dino.Units.Service;
using Feofun.Config;
using UnityEngine;
using Zenject;

namespace Dino.Core
{
    public class AnalyticsEventParamProvider: IEventParamProvider
    {
        [Inject] private SessionService _sessionService;
        [Inject] private PlayerProgressService _playerProgressService;
        [Inject] private StringKeyedConfigCollection<LevelMissionConfig> _levelsConfig;

        [Inject] private UnitService _unitService;
        [Inject] private World _world;
        
        
        public Dictionary<string, object> GetParams(IEnumerable<string> paramNames)
        {
            return paramNames.ToDictionary(it => it, GetValue);
        }

        private object GetValue(string paramName)
        {

            var playerProgress = _playerProgressService.Progress;
            
            return paramName switch
            {
                EventParams.LEVEL_ID => _sessionService.LevelId,
                EventParams.LEVEL_NUMBER => GetLevelNumber(),
                EventParams.LEVEL_LOOP => GetLevelLoop(),

                EventParams.ENEMY_KILLED => _sessionService.Kills.Value,
                EventParams.TIME_SINCE_LEVEL_START => _sessionService.SessionTime,
                EventParams.PASS_NUMBER => GetPassNumber(),
                EventParams.TOTAL_ENEMY_HEALTH => GetTotalEnemyHealth(),
                EventParams.AVERAGE_ENEMY_LIFETIME => GetAverageEnemyLifetime(),
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
            return Mathf.Max(0, playerProgress.LevelNumber - _levelsConfig.Keys.Count);
        }
        

        private int GetPassNumber()
        {
            var playerProgress = _playerProgressService.Progress;
            var levelConfig = _levelsConfig.Values[_sessionService.LevelId];
            return playerProgress.GetPassCount(levelConfig.Level);
        }
        
        private float GetTotalEnemyHealth()
        {
            var enemies = _unitService.GetEnemyUnits().ToList();
            return enemies
                .Select(it => it.Health)
                .Where(it => it != null).
                Sum(it => it.CurrentValue.Value);
        }

        private float GetAverageEnemyLifetime()
        {
            var enemies = _unitService.GetEnemyUnits().ToList();
            return enemies.Average(it => it.LifeTime);
        }
        
        private float GetStandRatio()
        {
            throw new NotImplementedException();
        }
    }
}
