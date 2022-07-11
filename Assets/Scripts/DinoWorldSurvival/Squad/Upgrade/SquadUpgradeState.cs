﻿using System.Collections.Generic;
using DinoWorldSurvival.Squad.Upgrade.Config;

namespace DinoWorldSurvival.Squad.Upgrade
{
    public class SquadUpgradeState
    {
        private readonly Dictionary<string, int> _upgradeLevels = new Dictionary<string, int>();

        public int GetLevel(string upgradeBranchId) => _upgradeLevels.ContainsKey(upgradeBranchId) ? _upgradeLevels[upgradeBranchId] : 0;

        public static SquadUpgradeState Create() => new SquadUpgradeState();

        public void IncreaseLevel(string upgradeBranchId)
        {
            if (!_upgradeLevels.ContainsKey(upgradeBranchId))
            {
                _upgradeLevels[upgradeBranchId] = 0;
            }

            _upgradeLevels[upgradeBranchId]++;
        }
        public bool IsMaxLevel(string upgradeBranchId, UpgradesConfig upgradesConfig)
        {
            return GetLevel(upgradeBranchId) >= upgradesConfig.GetMaxLevel(upgradeBranchId);
        }
        public IEnumerable<string> GetUpgradeBranchIds() => _upgradeLevels.Keys;
        public IReadOnlyDictionary<string, int> Upgrades => _upgradeLevels;
    }
}