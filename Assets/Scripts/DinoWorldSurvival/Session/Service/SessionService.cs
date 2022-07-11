using System.Linq;
using DinoWorldSurvival.App.Config;
using DinoWorldSurvival.Location;
using DinoWorldSurvival.Player.Progress.Model;
using DinoWorldSurvival.Player.Progress.Service;
using DinoWorldSurvival.Session.Config;
using DinoWorldSurvival.Session.Messages;
using DinoWorldSurvival.UI.Dialog.ReviveDialog;
using DinoWorldSurvival.Units;
using DinoWorldSurvival.Units.Service;
using Feofun.Config;
using Feofun.UI.Dialog;
using Logger.Extension;
using SuperMaxim.Messaging;
using UniRx;
using UnityEngine;
using Zenject;

namespace DinoWorldSurvival.Session.Service
{
    public class SessionService : IWorldScope
    {
        
        private readonly IntReactiveProperty _kills = new IntReactiveProperty(0);
        
        [Inject] private UnitFactory _unitFactory;     
        [Inject] private World _world;
        [Inject] private IMessenger _messenger;       
        [Inject] private UnitService _unitService;
        [Inject] private SessionRepository _repository;
        [Inject] private readonly StringKeyedConfigCollection<LevelMissionConfig> _levelsConfig;
        [Inject] private PlayerProgressService _playerProgressService;
        [Inject] private ConstantsConfig _constantsConfig;
        [Inject] private DialogManager _dialogManager;
        
        private CompositeDisposable _disposable;
        
        private PlayerProgress PlayerProgress => _playerProgressService.Progress;
        public Model.Session Session => _repository.Require();
        
        public IReadOnlyReactiveProperty<int> Kills => _kills;
        public LevelMissionConfig LevelConfig => _levelsConfig.Values[LevelId];
        public int LevelId => Mathf.Min(PlayerProgress.LevelNumber, _levelsConfig.Count() - 1);
        public float SessionTime => Session.SessionTime;
        public bool SessionCompleted => _repository.Exists() && Session.Completed;
        
        public void OnWorldSetup()
        {
            Dispose();
            _unitService.OnEnemyUnitDeath += OnEnemyUnitDeath;
            ResetKills();
            _disposable = new CompositeDisposable();
        }
        
        public void Start()
        {
            CreateSession();
            CreatePlayer();
        }

        private void CreateSession()
        {
            var levelConfig = LevelConfig;
            var newSession = Model.Session.Build(levelConfig);
            _repository.Set(newSession);
            _playerProgressService.OnSessionStarted(levelConfig.Level);
            this.Logger().Debug($"Kill enemies:= {levelConfig.KillCount}");
        }
    
        private void CreatePlayer()
        {
            var player = _unitFactory.CreatePlayerUnit(_constantsConfig.FirstUnit);
            _world.Player = player;
            player.OnDeath += OnDeath;
        }

        private void ResetKills() => _kills.Value = 0;
        private void OnEnemyUnitDeath(IUnit unit, DeathCause deathCause)
        {
            if (deathCause != DeathCause.Killed) return;
            
            Session.AddKill();
            _playerProgressService.AddKill();
            _kills.Value = Session.Kills;
            this.Logger().Trace($"Killed enemies:= {Session.Kills}");
            if (Session.IsMaxKills) {
                EndSession(UnitType.PLAYER);
            }
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
            _disposable?.Dispose();
            _disposable = null;
            _unitService.OnEnemyUnitDeath -= OnEnemyUnitDeath;
        }
        public void OnWorldCleanUp()
        {
            Dispose();
        }

        public void AddRevive()
        {
            Session.AddRevive();
        }
    }
}