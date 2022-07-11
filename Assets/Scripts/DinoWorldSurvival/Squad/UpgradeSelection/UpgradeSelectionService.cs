﻿using System.Collections.Generic;
using System.Linq;
using DinoWorldSurvival.Location;
using DinoWorldSurvival.Squad.Service;
using DinoWorldSurvival.Squad.Upgrade;
using DinoWorldSurvival.Squad.Upgrade.Config;
using DinoWorldSurvival.Squad.UpgradeSelection.Config;
using DinoWorldSurvival.UI.Dialog.PauseDialog;
using DinoWorldSurvival.UI.Dialog.UpgradeDialog;
using DinoWorldSurvival.UI.Dialog.UpgradeDialog.Model;
using Feofun.Extension;
using Feofun.UI.Dialog;
using Logger.Extension;
using ModestTree;
using UniRx;
using Zenject;

namespace DinoWorldSurvival.Squad.UpgradeSelection
{
    public class UpgradeSelectionService : IWorldScope
    {
        
        private const int PROPOSED_UPGRADE_COUNT = 3;

        [Inject]
        private SquadProgressService _squadProgressService;
        [Inject]
        private UpgradeBranchSelectionConfig _upgradeSelectionConfig;
        [Inject]
        private UpgradesConfig _upgradesConfig;
        [Inject]
        private DialogManager _dialogManager;
        [Inject]
        private SquadUpgradeRepository _repository;
        [Inject]
        private UpgradeService _upgradeService;
        [Inject]
        private World _world;
        [Inject] 
        private Analytics.Analytics _analytics;

        private CompositeDisposable _disposable;
        private SquadUpgradeState SquadUpgradeState => _repository.Require();

        public void OnWorldSetup()
        {
            _disposable?.Dispose();
            _disposable = new CompositeDisposable();
            _squadProgressService.Level.Subscribe(OnSquadLevelUpgrade).AddTo(_disposable);
        }
        
        private void OnSquadLevelUpgrade(int level)
        {
            if (level <= 1) {
                return;
            }
            TryShowUpgradeDialog(level);
        }

        private void TryShowUpgradeDialog(int level)
        {
            var randomUpgradeIds = GetRandomUpgradeIds(PROPOSED_UPGRADE_COUNT).ToList();
            if (randomUpgradeIds.IsEmpty()) {
                this.Logger().Info("Empty upgrades list");
                return;
            }
            _dialogManager.Show<UpgradeDialog, UpgradeDialogInitModel>(new UpgradeDialogInitModel(level, randomUpgradeIds, OnUpgrade));
            _world.Pause();
        }

        private void OnUpgrade(string upgradeBranchId)
        {
            _upgradeService.Upgrade(upgradeBranchId);
            _analytics.ReportLevelUp(upgradeBranchId);
            _dialogManager.Show<PauseDialog>();
        }

        private IEnumerable<string> GetRandomUpgradeIds(int upgradeCount)
        {
            var upgradeBranchIds = EnumExt.Values<UpgradeBranchType>().SelectMany(GetAvailableUpgradeBranchIds).ToList();
            return upgradeBranchIds.Count <= upgradeCount ? upgradeBranchIds : upgradeBranchIds.SelectRandomElements(upgradeCount);
        }

        private IEnumerable<string> GetAvailableUpgradeBranchIds(UpgradeBranchType branchType)
        {
            var upgradeBranchIds = _upgradesConfig.GetUpgradeBranchIds(branchType).ToList();
            var appliedBranchIds = upgradeBranchIds.Intersect(SquadUpgradeState.GetUpgradeBranchIds()).ToList();
            if (appliedBranchIds.Count >= _upgradeSelectionConfig.GetMaxUpgradeCount(branchType)) {
                upgradeBranchIds = appliedBranchIds;
            }
            return upgradeBranchIds.Where(id => !SquadUpgradeState.IsMaxLevel(id, _upgradesConfig));
        }
        
        public void OnWorldCleanUp()
        {
            Dispose();
        }
        private void Dispose()
        {
            _disposable?.Dispose();
            _disposable = null;
        }
    }
}