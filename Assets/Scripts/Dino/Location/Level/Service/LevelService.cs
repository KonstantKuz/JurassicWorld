﻿using System;
using System.Collections.Generic;
using System.Linq;
using Dino.Config;
using Dino.Location.Level.Config;
using Dino.Location.Service;
using Dino.Player.Progress.Model;
using Dino.Player.Progress.Service;
using Zenject;

namespace Dino.Location.Level.Service
{
    public class LevelService
    {
        private List<Level> _levels;

        [Inject] private WorldObjectFactory _worldObjectFactory;
        [Inject] private PlayerProgressService _playerProgressService;
        [Inject] private LevelsConfig _levelsConfig;   
        [Inject] private ConstantsConfig _constantsConfig;

        private PlayerProgress PlayerProgress => _playerProgressService.Progress;
        
        public string CurrentLevelId => GetLevelId(PlayerProgress.WinCount);
        public List<Level> Levels => _levels ??= _worldObjectFactory.GetPrefabComponents<Level>();

        private string GetLevelId(int winCount)
        {
            return winCount < _levelsConfig.Levels.Count ? _levelsConfig.Levels[winCount] : GetIterationLevelId(winCount);
        }

        private string GetIterationLevelId(int winCount)
        {
            if (_constantsConfig.IterationStartLevelIndex >= _levelsConfig.Levels.Count) {
                throw new ArgumentException("IterationStartLevelIndex must be < count of levels");
            }
            var iterationLevels = _levelsConfig.Levels.Skip(_constantsConfig.IterationStartLevelIndex).ToList(); 
            return iterationLevels[winCount % iterationLevels.Count];
        }

        public bool IsLastLevel(string levelId) => _levelsConfig.Levels.Last().Equals(levelId);

        public Level CreateLevel(string levelId)
        {
            var levelPrefab = Levels.FirstOrDefault(it => it.ObjectId == levelId);
            if (levelPrefab == null) {
                throw new ArgumentException($"LevelPrefab not found by levelId:= {levelId}");
            }
            return _worldObjectFactory.CreateObject<Level>(levelPrefab.gameObject);
        }
    }
}