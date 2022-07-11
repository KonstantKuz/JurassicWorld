using DinoWorldSurvival.Analytics;
using DinoWorldSurvival.Cheats.Installer;
using DinoWorldSurvival.Location;
using DinoWorldSurvival.Modifiers;
using DinoWorldSurvival.Player.Installer;
using DinoWorldSurvival.Reward.Installer;
using DinoWorldSurvival.UI;
using DinoWorldSurvival.Units.Installer;
using Feofun.Localization.Service;
using SuperMaxim.Messaging;
using UnityEngine;
using Zenject;

namespace DinoWorldSurvival.App
{
    public class MainSceneMonoInstaller : MonoInstaller
    {
        [SerializeField]
        private GameApplication _gameApplication;
        [SerializeField]
        private UpdateManager _updateManager;
        [SerializeField]
        private WorldServicesInstaller _worldServicesInstaller;  
        [SerializeField]
        private UIInstaller _uiInstaller;     
        [SerializeField]
        private CheatsInstaller _cheatsInstaller;

        public override void InstallBindings()
        {
            AnalyticsInstaller.Install(Container);
            Container.BindInterfacesTo<MainSceneMonoInstaller>().FromInstance(this).AsSingle();
            Container.Bind<GameApplication>().FromInstance(_gameApplication).AsSingle();
            Container.Bind<UpdateManager>().FromInstance(_updateManager).AsSingle();
            Container.Bind<IMessenger>().FromInstance(Messenger.Default).AsSingle();     
            Container.Bind<LocalizationService>().AsSingle();


            ConfigsInstaller.Install(Container);
            ModifiersInstaller.Install(Container);  
            
            UnitServicesInstaller.Install(Container);
            PlayerServicesInstaller.Install(Container);
            RewardServicesInstaller.Install(Container); 
            
            _worldServicesInstaller.Install(Container);
            _uiInstaller.Install(Container);
            _cheatsInstaller.Install(Container);
            
        }
    }
}