using Dino.Analytics.Wrapper;
using Dino.Core;
using Zenject;

namespace Dino.Analytics
{
    public class AnalyticsInstaller
    {
        public static void Install(DiContainer container)
        {
            container.Bind<Analytics>()
                .FromNew()
                .AsSingle()
                .WithArguments(new IAnalyticsImpl[]
                {
                        new LoggingAnalyticsWrapper(), 
                        /*new AppMetricaAnalyticsWrapper(),
                         new AppsFlyerAnalyticsWrapper(),*/
                        new FacebookAnalyticsWrapper()

                }).NonLazy();
            container.BindInterfacesTo<AnalyticsEventParamProvider>().AsSingle();
        }
    }
}