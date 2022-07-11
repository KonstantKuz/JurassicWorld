﻿using System.Runtime.Serialization;

namespace Dino.App.Config
{
    public class ConstantsConfig
    {
        [DataMember(Name = "HealthScaleIncrementFactor")]
        private float _healthScaleIncrementFactor;

        [DataMember(Name = "FirstUnit")]
        private string _firstUnit;    
        
        [DataMember(Name = "MaxMetaUpgradeLevel")]
        private int _maxMetaUpgradeLevel;

        [DataMember(Name = "ReviveEnemyRemoveRadius")]
        private float _reviveEnemyRemoveRadius;

        public float HealthScaleIncrementFactor => _healthScaleIncrementFactor;
        public string FirstUnit => _firstUnit;
        public int MaxMetaUpgradeLevel => _maxMetaUpgradeLevel;

        public float ReviveEnemyRemoveRadius => _reviveEnemyRemoveRadius;
    }
}