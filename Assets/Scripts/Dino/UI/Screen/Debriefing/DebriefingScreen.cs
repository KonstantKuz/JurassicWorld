using Dino.Inventory.Service;
using Dino.Location.Level.Service;
using Dino.Reward.Service;
using Dino.Session.Model;
using Dino.UI.Screen.Debriefing.Model;
using Dino.UI.Screen.Main;
using Dino.UI.Screen.Menu;
using Feofun.UI.Components.Button;
using Feofun.UI.Screen;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace Dino.UI.Screen.Debriefing
{
    public class DebriefingScreen : BaseScreen
    {
        public const ScreenId ID = ScreenId.Debriefing;
        public override ScreenId ScreenId => ID; 
        
        public static readonly string URL = MenuScreen.ID + "/" + ID;
        public override string Url => URL;

        [SerializeField]
        private SessionResultPanel _resultPanel;
        [SerializeField]
        private ActionButton _nextButton;    
        [SerializeField]
        private ActionButton _reloadButton;

        [Inject] private ScreenSwitcher _screenSwitcher;       
        [Inject] private MissionResultRewardService _missionResultRewardService;  
        [Inject] private IRewardApplyService _rewardApplyService;      
        [Inject] private InventoryService _inventoryService;     
        [Inject] private LevelService _levelService;

        [PublicAPI]
        public void Init(DebriefingScreenModel model)
        {
            _nextButton.gameObject.SetActive(model.SessionResult == SessionResult.Win);     
            _reloadButton.gameObject.SetActive(model.SessionResult == SessionResult.Lose);

            var rewards = _missionResultRewardService.CalculateRewards(model.SessionResult, model.Session);
            _rewardApplyService.ApplyRewards(rewards);

            var resultPanelModel = model.BuildResultPanelModel(rewards);
            _resultPanel.Init(resultPanelModel);
            SaveOrDeleteInventory(model.Session);
        }
        private void SaveOrDeleteInventory(Session.Model.Session session)
        {
            if (session.Result == SessionResult.Win) {
                _inventoryService.Save();
            }
            if (session.Result == SessionResult.Win && _levelService.IsLastLevel(session.LevelId)) {
                _inventoryService.Delete();
            }
        }
        public void OnEnable()
        {
            _nextButton.Init(OnReload);
            _reloadButton.Init(OnReload);
        }

        private void OnReload()
        {
            _screenSwitcher.SwitchTo(MainScreen.URL);
        }
    }
}