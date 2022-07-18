﻿using System;
using System.Collections.Generic;
using System.Linq;
using Dino.Extension;
using Dino.Player.Progress.Model;
using Dino.Player.Progress.Service;
using Logger.Extension;
using UnityEngine;
using Zenject;

namespace Dino.Location.Service
{
    public class LevelService
    {
        private List<Level> _levels;
        private Level _currentLevel;

        [Inject] private WorldObjectFactory _worldObjectFactory;
        [Inject] private PlayerProgressService _playerProgressService;

        private PlayerProgress PlayerProgress => _playerProgressService.Progress;
        
        public int CurrentLevelId => GetLevelIndexByWinCount(Levels, PlayerProgress.WinCount);
        public List<Level> Levels => _levels ??= _worldObjectFactory.GetPrefabComponents<Level>();
        public Level CurrentLevel => _currentLevel;

        private int GetLevelIndexByWinCount(List<Level> levels, int winCount)
        {
            var levelIndex = 0;
            for (int i = 0; i < winCount; i++)
            {
                levelIndex = levelIndex >= levels.Count - 1 ? 0 : levelIndex + 1;
            }
            return levelIndex;
        }

        public void CreateLevel()
        {
            var levelPrefab = Levels[CurrentLevelId].gameObject;
            _currentLevel = _worldObjectFactory.CreateObject(levelPrefab).RequireComponent<Level>();
        }
    }
}