﻿using System.Linq;
using Dino.ABTest;
using Dino.Cheats;
using Dino.Inventory.Config;
using Dino.Weapon.Config;
using Feofun.Cheats;
using Feofun.Config;
using Feofun.Extension;
using Feofun.UI.Components.Button;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Dino.UI.Cheats
{
    public class CheatsScreenPresenter : MonoBehaviour
    {
        [SerializeField] private ActionButton _closeButton;
        [SerializeField] private ActionButton _hideButton;

        [SerializeField] private ActionToggle _toggleConsoleButton;
        [SerializeField] private ActionToggle _toggleFPSButton;
        [SerializeField] private ActionToggle _toggleAdsButton;   
        [SerializeField] private ActionToggle _toggleABTestButton; 

        [SerializeField] private ActionButton _resetProgressButton;

        [SerializeField] private InputField _inputField; [SerializeField]
        private ActionButton _setLanguage;

        [SerializeField] private ActionButton _setRussianLanguage;
        [SerializeField] private ActionButton _setEnglishLanguage;

        [SerializeField] private ActionButton _testLogButton;     
        
        [SerializeField] private ActionButton _analyticsTestButton;

        [SerializeField] private DropdownWithButtonView _inventoryDropdown;
        [SerializeField] private ActionButton _removeActiveItemButton; 
        [SerializeField] private ActionButton _removeInventoryItemButton;   
        [SerializeField] private ActionButton _addInventoryItemButton;       
                
        [SerializeField] private DropdownWithButtonView _craftDropdown;
        [SerializeField] private DropdownWithButtonView _abTestDropdown;

        [Inject] private CheatsManager _cheatsManager;
        [Inject] private CheatsActivator _cheatsActivator;
        [Inject] private StringKeyedConfigCollection<WeaponConfig> _weaponConfigs;    
        [Inject] private CraftConfig _craftConfig;

        private void OnEnable()
        {
            _closeButton.Init(HideCheatsScreen);
            _hideButton.Init(DisableCheats);

            _toggleConsoleButton.Init(_cheatsManager.IsConsoleEnabled, value => { _cheatsManager.IsConsoleEnabled = value; });
            _toggleFPSButton.Init(_cheatsManager.IsFPSMonitorEnabled, value => { _cheatsManager.IsFPSMonitorEnabled = value; });
            _toggleAdsButton.Init(_cheatsManager.IsAdsCheatEnabled, value => _cheatsManager.IsAdsCheatEnabled = value);
            _toggleABTestButton.Init(_cheatsManager.IsABTestCheatEnabled, value => _cheatsManager.IsABTestCheatEnabled = value);

            _resetProgressButton.Init(_cheatsManager.ResetProgress);

            _setLanguage.Init(() => _cheatsManager.SetLanguage(_inputField.text));
            _setEnglishLanguage.Init(() => _cheatsManager.SetLanguage(SystemLanguage.English.ToString()));
            _setRussianLanguage.Init(() => _cheatsManager.SetLanguage(SystemLanguage.Russian.ToString()));
            _testLogButton.Init(_cheatsManager.LogTestMessage);
            _analyticsTestButton.Init(_cheatsManager.ReportAnalyticsTestEvent);
                    
            _inventoryDropdown.Init(_weaponConfigs.Keys.Select(it => it.ToString()).ToList(), _cheatsManager.SetActiveWeapon);
            _removeActiveItemButton.Init(_cheatsManager.RemoveActiveItem);
            
            _removeInventoryItemButton.Init(() => _cheatsManager.RemoveInventoryWeapon(_inventoryDropdown.CurrentValue));
            _addInventoryItemButton.Init(() => _cheatsManager.AddInventoryWeapon(_inventoryDropdown.CurrentValue));
            
            _craftDropdown.Init(_craftConfig.Crafts.Keys.ToList(), _cheatsManager.Craft);
            _abTestDropdown.Init(EnumExt.Values<ABTestVariantId>().Select(it => it.ToCamelCase()).ToList(), _cheatsManager.SetCheatAbTest);
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