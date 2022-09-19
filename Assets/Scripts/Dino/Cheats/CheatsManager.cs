using System;
using Dino.Cheats.Data;
using Dino.Cheats.Repository;
using Dino.Inventory.Model;
using Dino.Inventory.Service;
using Dino.Units.Service;
using Feofun.ABTest;
using Feofun.ABTest.Providers;
using Feofun.Advertisment.Providers;
using Feofun.Advertisment.Service;
using Feofun.Localization.Service;
using Logger.Extension;
using UnityEngine;
using Zenject;

namespace Dino.Cheats
{
    public class CheatsManager : MonoBehaviour, IABTestCheatManager
    {
        private const string TEST_LOG_MESSAGE = "Test log message";
        private readonly CheatRepository _repository = new CheatRepository();
        
        [Inject] private LocalizationService _localizationService;    
        [Inject] private ActiveItemService _activeItemService;     
        [Inject] private InventoryService _inventoryService;    
        [Inject] private CraftService _craftService;     
        [Inject] private Analytics.Analytics _analytics;     
        [Inject] private Feofun.ABTest.ABTest _abTest;
        [Inject(Optional = true)] private AdsManager _adsManager;    
        [Inject] private DiContainer _diContainer;           

        [SerializeField] private GameObject _fpsMonitor;
        [SerializeField] private GameObject _debugConsole;
        
        private CheatSettings Settings => _repository.Get() ?? new CheatSettings();
        
        private void Awake()
        {
#if DEBUG_CONSOLE_ENABLED
            IsConsoleEnabled = true;
#endif
            _debugConsole.SetActive(IsConsoleEnabled); 
            _fpsMonitor.SetActive(IsFPSMonitorEnabled);
        }
        public void ResetProgress()
        {
            PlayerPrefs.DeleteAll();
            Application.Quit();
        }

        public void ReportAnalyticsTestEvent()
        {
            _analytics.ReportTest();
        }

        public void SetActiveWeapon(string itemName)
        {
            _activeItemService.Replace(_inventoryService.GetItem(ItemId.Create(itemName)));
        } 
        public void RemoveActiveItem()
        {
            _activeItemService.UnEquip();
        }      
        public void RemoveInventoryWeapon(string itemName)
        { 
            _inventoryService.Remove(ItemId.Create(itemName));
        }  
        public void AddInventoryWeapon(string itemName)
        { 
            _inventoryService.Add(ItemId.Create(itemName), InventoryItemType.Weapon, 1);
        }   
        public void Craft(string craftItemId)
        { 
            _craftService.Craft(craftItemId);
        }
        
        public void LogTestMessage()
        {
            var logger = this.Logger();
            logger.Trace(TEST_LOG_MESSAGE);
            logger.Debug(TEST_LOG_MESSAGE);      
            logger.Info(TEST_LOG_MESSAGE);
            logger.Warn(TEST_LOG_MESSAGE);     
            logger.Error(TEST_LOG_MESSAGE);
        }

        public void SetLanguage(string language)
        {
            _localizationService.SetLanguageOverride(language);
        }
        private void UpdateSettings(Action<CheatSettings> updateFunc)
        {
            var settings = Settings;
            updateFunc?.Invoke(settings);
            _repository.Set(settings);
        }
        public bool IsConsoleEnabled
        {
            get => Settings.ConsoleEnabled;
            set
            {
                UpdateSettings(settings => { settings.ConsoleEnabled = value; });
                _debugConsole.SetActive(value);
            }
        }    
        public bool IsFPSMonitorEnabled
        {
            get => Settings.FPSMonitorEnabled;
            set
            {
                UpdateSettings(settings => { settings.FPSMonitorEnabled = value; });
                _fpsMonitor.SetActive(value);
            }
        }
        
        public void SetCheatAbTest(string variantId)
        {
            OverrideABTestProvider.SetVariantId(variantId);
            _abTest.Reload();
        }
        
        public bool IsAdsCheatEnabled  {
            get => _adsManager?.AdsProvider is CheatAdsProvider;
            set 
            {
                if (_adsManager == null) return;
                _adsManager.AdsProvider = value ? new CheatAdsProvider() : _diContainer.Resolve<IAdsProvider>(); 
            }
        } 
        
        public bool IsABTestCheatEnabled
        {
            get => Settings.ABTestCheatEnabled;
            set
            {
                UpdateSettings(settings => { settings.ABTestCheatEnabled = value; });
                _abTest.Reload();
            }
        }    
        
    }
}

