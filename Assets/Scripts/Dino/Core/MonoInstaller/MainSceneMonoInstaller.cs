using Dino.ABTest;
using Dino.Advertisment;
using Dino.Analytics;
using Dino.Cheats.Installer;
using Dino.Config;
using Dino.Inventory.Installer;
using Dino.Location.Installer;
using Dino.Modifiers;
using Dino.Player.Installer;
using Dino.Reward.Installer;
using Dino.Tutorial.Installer;
using Dino.UI;
using Dino.Units.Installer;
using Feofun.ABTest.Installer;
using Feofun.Localization.Service;
using Feofun.ReceivingLoot;
using SuperMaxim.Messaging;
using UnityEngine;

namespace Dino.Core.MonoInstaller
{
    public class MainSceneMonoInstaller : Zenject.MonoInstaller
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
        [SerializeField]
        private TutorialInstaller _tutorialInstaller;

        public override void InstallBindings()
        {
            AnalyticsInstaller.Install(Container);
            Container.BindInterfacesTo<MainSceneMonoInstaller>().FromInstance(this).AsSingle();
            Container.Bind<GameApplication>().FromInstance(_gameApplication).AsSingle();
            Container.Bind<UpdateManager>().FromInstance(_updateManager).AsSingle();
            Container.Bind<IMessenger>().FromInstance(Messenger.Default).AsSingle();     
            Container.Bind<LocalizationService>().AsSingle();
            Container.Bind<FlyingIconReceivingManager>().AsSingle();

            ConfigsInstaller.Install(Container);
            ModifiersInstaller.Install(Container);  
            
            UnitServicesInstaller.Install(Container);
            InventoryServicesInstaller.Install(Container);
            PlayerServicesInstaller.Install(Container);
            RewardServicesInstaller.Install(Container);
            ABTestServicesInstaller.Install(Container, ABTestVariantId.WithoutAmmo);

            _tutorialInstaller.Install(Container);
            _worldServicesInstaller.Install(Container);
            _uiInstaller.Install(Container);
            _cheatsInstaller.Install(Container);
        }
    }
}
