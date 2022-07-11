using DinoWorldSurvival.Squad.Progress;
using DinoWorldSurvival.Squad.Service;
using DinoWorldSurvival.Squad.Upgrade;
using DinoWorldSurvival.Squad.UpgradeSelection;
using Zenject;

namespace DinoWorldSurvival.Squad.Installer
{
    public class SquadServicesInstaller
    {
        public static void Install(DiContainer container)
        {
            container.Bind<SquadFactory>().AsSingle();
            container.BindInterfacesAndSelfTo<SquadProgressService>().AsSingle();
            container.BindInterfacesAndSelfTo<UpgradeService>().AsSingle();
            container.BindInterfacesAndSelfTo<UpgradeSelectionService>().AsSingle();           
            
            container.BindInterfacesAndSelfTo<MetaUpgradeService>().AsSingle();   
            
            container.Bind<SquadProgressRepository>().AsSingle();           
            container.Bind<SquadUpgradeRepository>().AsSingle();  
        }
    }
}
