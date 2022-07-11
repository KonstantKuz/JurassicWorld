using System;
using DinoWorldSurvival.Cheats.Data;
using DinoWorldSurvival.Cheats.Repository;
using DinoWorldSurvival.Squad.Service;
using DinoWorldSurvival.Squad.Upgrade;
using Feofun.Localization.Service;
using Logger.Extension;
using UnityEngine;
using Zenject;

namespace DinoWorldSurvival.Cheats
{
    public class CheatsManager : MonoBehaviour
    {
        private const string TEST_LOG_MESSAGE = "Test log message";
        private readonly CheatRepository _repository = new CheatRepository();
        
        [Inject] private LocalizationService _localizationService;     
        [Inject] private SquadProgressService _squadProgressService;
        [Inject] private UpgradeService _upgradeService;
        [Inject] private MetaUpgradeService _metaUpgradeService;

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
        
        public void IncreaseSquadLevel() => _squadProgressService.IncreaseLevel();
        public void AddRandomSquadUpgrade() => _upgradeService.AddRandomUpgrade();   
        public void ApplyAllSquadUpgrades() => _upgradeService.ApplyAllUpgrades();  
        public void AddUnit(string unitId) => _upgradeService.AddUnit(unitId);
        public void AddMetaUpgrade(string upgradeId) => _metaUpgradeService.Upgrade(upgradeId);

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
    }
}

