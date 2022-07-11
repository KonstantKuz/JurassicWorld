﻿using DinoWorldSurvival.Cheats;
using DinoWorldSurvival.Config;
using DinoWorldSurvival.Modifiers.Config;
using DinoWorldSurvival.Units.Player.Config;
using Feofun.Cheats;
using Feofun.Config;
using Feofun.UI.Components.Button;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace DinoWorldSurvival.UI.Cheats
{
    public class CheatsScreenPresenter : MonoBehaviour
    {
        [SerializeField] private ActionButton _closeButton;
        [SerializeField] private ActionButton _hideButton;

        [SerializeField] private ActionToggle _toggleConsoleButton;
        [SerializeField] private ActionToggle _toggleFPSButton;

        [SerializeField] private ActionButton _increaseSquadLevelButton; 
        [SerializeField] private ActionButton _applyAllUpgradesButton;
        [SerializeField] private ActionButton _addRandomSquadUpgrade; 
        [SerializeField] private ActionButton _resetProgressButton;

        [SerializeField] private InputField _inputField;
        [SerializeField] private ActionButton _setLanguage;

        [SerializeField] private ActionButton _setRussianLanguage;
        [SerializeField] private ActionButton _setEnglishLanguage;    
        
        [SerializeField] private ActionButton _testLogButton;    
        
        [SerializeField] private DropdownWithButtonView _addUnitsView;
        [SerializeField] private DropdownWithButtonView _addMetaUpgradeView;

        [Inject] private CheatsManager _cheatsManager;
        [Inject] private CheatsActivator _cheatsActivator;
        
        [Inject]
        private StringKeyedConfigCollection<PlayerUnitConfig> _playerUnitConfigs;     
        [Inject(Id = Configs.META_UPGRADES)]
        private StringKeyedConfigCollection<ParameterUpgradeConfig> _modifierConfigs;

        private void OnEnable()
        {
            _addUnitsView.Init(_playerUnitConfigs.Keys, _cheatsManager.AddUnit);   
            _addMetaUpgradeView.Init(_modifierConfigs.Keys, _cheatsManager.AddMetaUpgrade);
            
            _closeButton.Init(HideCheatsScreen);
            _hideButton.Init(DisableCheats);

            _toggleConsoleButton.Init(_cheatsManager.IsConsoleEnabled, value => { _cheatsManager.IsConsoleEnabled = value; });
            _toggleFPSButton.Init(_cheatsManager.IsFPSMonitorEnabled, value => { _cheatsManager.IsFPSMonitorEnabled = value; });
          
            _increaseSquadLevelButton.Init(_cheatsManager.IncreaseSquadLevel);     
            _addRandomSquadUpgrade.Init(_cheatsManager.AddRandomSquadUpgrade);
            _applyAllUpgradesButton.Init(_cheatsManager.ApplyAllSquadUpgrades);
            _resetProgressButton.Init(_cheatsManager.ResetProgress);
            
            _setLanguage.Init(() => _cheatsManager.SetLanguage(_inputField.text));
            _setEnglishLanguage.Init(() => _cheatsManager.SetLanguage(SystemLanguage.English.ToString()));
            _setRussianLanguage.Init(() => _cheatsManager.SetLanguage(SystemLanguage.Russian.ToString()));
            _testLogButton.Init(() => _cheatsManager.LogTestMessage());
        }
        private void DisableCheats()
        {
            _cheatsActivator.ShowOpenCheatButton(false);
            _cheatsActivator.EnableCheats(false);
            HideCheatsScreen();
        }
        private void HideCheatsScreen() => _cheatsActivator.ShowCheatsScreen(false);
    }
}