using DinoWorldSurvival.Units.Service;
using DinoWorldSurvival.Units.Target;
using Zenject;

namespace DinoWorldSurvival.Units.Installer
{
    public class UnitServicesInstaller
    {
        public static void Install(DiContainer container)
        {
            container.Bind<UnitFactory>().AsSingle();
            container.Bind<TargetService>().AsSingle();
            container.Bind<UnitService>().AsSingle();     
            container.Bind<PlayerUnitModelBuilder>().AsSingle();
        }
    }
}