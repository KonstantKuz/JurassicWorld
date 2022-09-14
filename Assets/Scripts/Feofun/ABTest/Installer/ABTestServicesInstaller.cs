using System.Collections.Generic;
using System.Linq;
using Feofun.ABTest.Providers;
using Feofun.Extension;
using Zenject;

namespace Feofun.ABTest.Installer
{
    public class ABTestServicesInstaller
    {
        public static void Install(DiContainer container, string controlVariant, IEnumerable<string> abVariants)
        {
            container.Bind<ABTest>().AsSingle();
            container.Bind<IABTestProvider>()
                .To<OverrideABTestProvider>()
                .AsSingle()
                .WithArguments(new YCABTestProvider(controlVariant, abVariants), controlVariant);
        }

        public static void Install<T>(DiContainer container, T controlVariant) where T: struct //enum
        {
            Install(container, ToCamelCase(controlVariant), EnumExt.Values<T>().Select(ToCamelCase));
        }
        
        private static string ToCamelCase<T>(T enumValue) where T: struct
        {
            var variantId = enumValue.ToString();
            return char.ToLower(variantId[0]) + variantId.Substring(1);
        }
    }
}