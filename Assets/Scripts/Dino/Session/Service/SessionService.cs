using Dino.Config;
using Dino.Inventory.Service;
using Dino.Location;
using Dino.Location.Service;
using Dino.Player.Progress.Service;
using Dino.Session.Messages;
using Dino.Units;
using Dino.Units.Service;
using Logger.Extension;
using SuperMaxim.Messaging;
using UniRx;
using Zenject;
using Unit = Dino.Units.Unit;

namespace Dino.Session.Service
{
    public class SessionService : IWorldScope
    {
        private readonly IntReactiveProperty _kills = new IntReactiveProperty(0);

        private Level _currentLevel;
        
        [Inject] private LevelService _levelService;
        [Inject] private EnemyInitService _enemyInitService;
        [Inject] private UnitFactory _unitFactory;     
        [Inject] private World _world;
        [Inject] private IMessenger _messenger;       
        [Inject] private UnitService _unitService;
        [Inject] private SessionRepository _repository;
        [Inject] private PlayerProgressService _playerProgressService;
        [Inject] private ConstantsConfig _constantsConfig;
        [Inject] private ActiveItemService _activeItemService;

        public int CurrentLevelId => _levelService.CurrentLevelId;
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
            var newSession = Model.Session.Build(CurrentLevelId);
            _repository.Set(newSession);
            _playerProgressService.OnSessionStarted(CurrentLevelId);
        }

        private void CreateLevel()
        {
            _currentLevel = _levelService.CreateCurrentLevel();
            _currentLevel.OnPlayerTriggeredFinish += OnFinishTriggered;
            this.Logger().Debug($"Level:= {_currentLevel.gameObject.name}");
        }

        private void OnFinishTriggered()
        {
            EndSession(UnitType.PLAYER);
        }

        private void CreatePlayer()
        {
            var player = _unitFactory.CreatePlayerUnit(_constantsConfig.FirstUnit, _currentLevel.Start.position);
            _world.Player = player;
            player.OnDeath += OnDeath;
            
            _activeItemService.Equip(_constantsConfig.FirstItem);
        }

        private void InitEnemies()
        {
            _enemyInitService.InitEnemies();
        }

        private void ResetKills() => _kills.Value = 0;
        private void OnEnemyUnitDeath(Unit unit, DeathCause deathCause)
        {
            if (deathCause != DeathCause.Killed) return;
            
            Session.AddKill();
            _playerProgressService.AddKill();
            _kills.Value = Session.Kills;
            this.Logger().Trace($"Killed enemies:= {Session.Kills}");
        }

        private void OnDeath(Unit unit, DeathCause deathCause)
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
            if (_currentLevel != null) {
                _currentLevel.OnPlayerTriggeredFinish -= OnFinishTriggered;
            }
        }
        public void OnWorldCleanUp()
        {
            Dispose();
            _currentLevel = null;
        }
    }
}