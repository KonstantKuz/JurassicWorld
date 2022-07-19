using System.Collections.Generic;
using Dino.Extension;
using Dino.Player.Progress.Model;
using Dino.Player.Progress.Service;
using Zenject;

namespace Dino.Location.Service
{
    public class LevelService
    {
        private List<Level> _levels;

        [Inject] private WorldObjectFactory _worldObjectFactory;
        [Inject] private PlayerProgressService _playerProgressService;

        private PlayerProgress PlayerProgress => _playerProgressService.Progress;
        
        public int CurrentLevelId => GetLevelIndexByWinCount(Levels, PlayerProgress.WinCount);
        public List<Level> Levels => _levels ??= _worldObjectFactory.GetPrefabComponents<Level>();

        private int GetLevelIndexByWinCount(List<Level> levels, int winCount)
        {
            var levelIndex = 0;
            for (int i = 0; i < winCount; i++)
            {
                levelIndex = levelIndex >= levels.Count - 1 ? 0 : levelIndex + 1;
            }
            return levelIndex;
        }

        public Level CreateCurrentLevel()
        {
            var levelPrefab = Levels[CurrentLevelId].gameObject;
            return _worldObjectFactory.CreateObject(levelPrefab).RequireComponent<Level>();
        }
    }
}