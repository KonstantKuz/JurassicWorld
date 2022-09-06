using Dino.ABTest.Providers;
using Zenject;

namespace Dino.ABTest.Installer
{
    public class ABTestServicesInstaller
    {
        public static void Install(DiContainer container)
        {
            container.Bind<Dino.ABTest.ABTest>().AsSingle();
            container.Bind<IABTestProvider>().To<OverrideABTestProvider>().AsSingle().WithArguments(new YCABTestProvider());
        }
    }
}