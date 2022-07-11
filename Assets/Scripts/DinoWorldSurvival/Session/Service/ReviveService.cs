using System.Linq;
using DinoWorldSurvival.App.Config;
using DinoWorldSurvival.Location;
using DinoWorldSurvival.Units;
using DinoWorldSurvival.Units.Service;
using ModestTree;
using Zenject;

namespace DinoWorldSurvival.Session.Service
{
    public class ReviveService
    {
        [Inject] private World _world;     
        [Inject] private UnitService _unitService;
        [Inject] private ConstantsConfig _constantsConfig;  
        [Inject] private SessionService _sessionService;

        public void Revive()
        {
            Assert.IsNotNull(_world.Squad, "Should call this method only inside game session");
            _world.Squad.RestoreHealth();
            KillEnemiesAroundSquad();
            _sessionService.AddRevive();
        }
        
        private void KillEnemiesAroundSquad()
        {
            var enemiesNearby = _unitService.GetUnitsInRadius(_world.Squad.Position, UnitType.ENEMY,
                _constantsConfig.ReviveEnemyRemoveRadius).ToList();
            enemiesNearby.ForEach(it => it.Kill(DeathCause.Removed));
        }        
    }
}