﻿using System;
using System.Linq;
using DinoWorldSurvival.App.Config;
using DinoWorldSurvival.Location;
using DinoWorldSurvival.Units;
using DinoWorldSurvival.Units.Service;
using UnityEngine;
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
            throw new NotImplementedException();
            KillEnemiesAroundSquad();
            _sessionService.AddRevive();
        }
        
        private void KillEnemiesAroundSquad()
        {
            throw new NotImplementedException();
            var playerPosition = Vector3.zero;
            var enemiesNearby = _unitService.GetUnitsInRadius(playerPosition, UnitType.ENEMY,
                _constantsConfig.ReviveEnemyRemoveRadius).ToList();
            enemiesNearby.ForEach(it => it.Kill(DeathCause.Removed));
        }        
    }
}