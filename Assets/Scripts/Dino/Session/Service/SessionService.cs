using System.Collections.Generic;
using System.Linq;
using Dino.Config;
using Dino.Extension;
using Dino.Inventory.Service;
using Dino.Location;
using Dino.Location.Service;
using Dino.Player.Progress.Model;
using Dino.Player.Progress.Service;
using Dino.Session.Messages;
using Dino.Units;
using Dino.Units.Service;
using Logger.Extension;
using ModestTree;
using SuperMaxim.Messaging;
using UniRx;
using UnityEngine;
using Zenject;

namespace Dino.Session.Service
{
    public class SessionService : IWorldScope
    {
        private const string LEVELS_PATH = "Content";
        private readonly IntReactiveProperty _kills = new IntReactiveProperty(0);

        [Inject] private WorldObjectFactory _worldObjectFactory;
        [Inject] private EnemyInitService _enemyInitService;
        [Inject] private UnitFactory _unitFactory;     
        [Inject] private World _world;
        [Inject] private IMessenger _messenger;       
        [Inject] private UnitService _unitService;
        [Inject] private SessionRepository _repository;
        [Inject] private PlayerProgressService _playerProgressService;
        [Inject] private ConstantsConfig _constantsConfig;
        [Inject] private ActiveItemService _activeItemService;

        private Level _currentLevel;
        private List<Level> _levels;
        
        private PlayerProgress PlayerProgress => _playerProgressService.Progress;

        public List<Level> Levels => _levels ??= Resources.LoadAll<Level>(LEVELS_PATH).ToList();
        public int LevelId => GetLevelIndexByWinCount(Levels, PlayerProgress.WinCount);
        public Model.Session Session => _repository.Require();
        public IReadOnlyReactiveProperty<int> Kills => _kills;
        public float SessionTime => Session.SessionTime;
        public bool SessionCompleted => _repository.Exists() && Session.Completed;
        
        public void OnWorldSetup()
        {
            Dispose();
            _unitService.OnEnemyUnitDeath += OnEnemyUnitDeath;
            ResetKills();
        }
        
        public void Start()
        {
            CreateSession();
            CreateLevel();
            CreatePlayer();
            InitEnemies();
        }

        private void CreateSession()
        {
            var newSession = Model.Session.Build(LevelId);
            _repository.Set(newSession);
            _playerProgressService.OnSessionStarted(LevelId);
        }

        private void CreateLevel()
        {
            var levelPrefab = Levels[LevelId].gameObject;
            _currentLevel = _worldObjectFactory.CreateObject(levelPrefab).RequireComponent<Level>();
            _currentLevel.OnPlayerTriggeredFinish += OnFinishTriggered;
            this.Logger().Debug($"Level:= {_currentLevel.gameObject.name}");
        }
        
        private int GetLevelIndexByWinCount(List<Level> levels, int winCount)
        {
            var levelIndex = 0;
            for (int i = 0; i < winCount; i++)
            {
                levelIndex = levelIndex >= levels.Count - 1 ? 0 : levelIndex + 1;
            }
            return levelIndex;
        }

        private void OnFinishTriggered()
        {
            EndSession(UnitType.PLAYER);
        }

        private void CreatePlayer()
        {
            Assert.That(_currentLevel != null, "Level should be spawned before player.");
            var player = _unitFactory.CreatePlayerUnit(_constantsConfig.FirstUnit);
            player.transform.position = _currentLevel.Start.position;
            _world.Player = player;
            player.OnDeath += OnDeath;
            _activeItemService.Set(_constantsConfig.FirstItem);
        }

        private void InitEnemies()
        {
            _enemyInitService.InitEnemies();
        }

        private void ResetKills() => _kills.Value = 0;
        private void OnEnemyUnitDeath(IUnit unit, DeathCause deathCause)
        {
            if (deathCause != DeathCause.Killed) return;
            
            Session.AddKill();
            _playerProgressService.AddKill();
            _kills.Value = Session.Kills;
            this.Logger().Trace($"Killed enemies:= {Session.Kills}");
        }

        private void OnDeath(IUnit unit, DeathCause deathCause)
        {
            EndSession(UnitType.ENEMY);
        }
        
        private void EndSession(UnitType winner)
        {
            Dispose();
            Session.SetResultByUnitType(winner);
            
            _unitService.DeactivateAll();
            
            _messenger.Publish(new SessionEndMessage(Session.Result.Value));
        }
        
        private void Dispose()
        {
            _unitService.OnEnemyUnitDeath -= OnEnemyUnitDeath;
        }
        public void OnWorldCleanUp()
        {
            Dispose();
        }
    }
}