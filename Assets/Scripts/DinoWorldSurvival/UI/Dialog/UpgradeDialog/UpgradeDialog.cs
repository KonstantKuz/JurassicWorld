using DinoWorldSurvival.Squad.Upgrade;
using DinoWorldSurvival.Squad.Upgrade.Config;
using DinoWorldSurvival.UI.Dialog.UpgradeDialog.Model;
using DinoWorldSurvival.UI.Dialog.UpgradeDialog.View;
using UnityEngine;
using Zenject;

namespace DinoWorldSurvival.UI.Dialog.UpgradeDialog
{
    public class UpgradeDialog : BaseDialog, IUiInitializable<UpgradeDialogInitModel>
    {
        [SerializeField]
        private UpgradeView _view;

        [Inject] private DialogManager _dialogManager;
        [Inject] private UpgradeService _upgradeService;
        [Inject] private UpgradesConfig _upgradesConfig;
        [Inject(Id = Configs.MODIFIERS)]
        private StringKeyedConfigCollection<ParameterUpgradeConfig> _modifierConfigs;
        
        private UpgradeDialogModel _model;

        public void Init(UpgradeDialogInitModel initModel)
        {
            _model = new UpgradeDialogModel(initModel, _upgradesConfig, _upgradeService.SquadUpgradeState, _modifierConfigs, OnUpgrade);
            _view.Init(_model);
        }

        private void OnUpgrade(string upgradeBranchId)
        {
            _model.InitModel.OnUpgrade?.Invoke(upgradeBranchId);
            _dialogManager.Hide<UpgradeDialog>();
        }

        private void OnDisable()
        {
            _model = null;
        }
    }
}