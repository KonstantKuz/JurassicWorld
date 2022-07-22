﻿using Dino.Units;
using UnityEngine;

namespace Dino.Session.Model
{
    public class Session
    {
        private readonly float _startTime;
        private int _maxKillsCount;
        public int LevelId { get; private set; }
        public int Kills { get; private set; }
        public SessionResult? Result { get; private set; }
        public int Revives { get; private set; }
        public bool IsMaxKills => Kills >= _maxKillsCount;
        public bool Completed => Result.HasValue;

        
        private Session(int levelId)
        {
            _startTime = Time.time;
            LevelId = levelId;
        }
        public static Session Build(int levelId) => new Session(levelId);

        public void SetMaxKillsCount(int count)
        {
            _maxKillsCount = count;
        }
        
        public void SetResultByUnitType(UnitType unitType)
        {
            Result = unitType == UnitType.PLAYER ? SessionResult.Win : SessionResult.Lose;
        }
        public void AddKill() => Kills++;

        public void AddRevive() => Revives++;
        
        public float SessionTime => Time.time - _startTime;        
    }
}