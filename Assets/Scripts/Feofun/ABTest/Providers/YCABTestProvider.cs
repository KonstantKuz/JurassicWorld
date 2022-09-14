using System.Collections.Generic;
using System.Linq;
using Logger.Extension;
using YsoCorp.GameUtils;

namespace Feofun.ABTest.Providers
{
    public class YCABTestProvider : IABTestProvider
    {
        private readonly string _controlVariant;
        private readonly List<string> _variants;

        public YCABTestProvider(string controlVariant, IEnumerable<string> variants)
        {
            _controlVariant = controlVariant;
            _variants = variants.ToList();
        }

        public string GetVariant()
        {
            foreach (var variantId in _variants) {
                if (!IsVariantId(variantId)) { //should check YCManager.IsPlayerSample(variantId), because YCManager.GetPlayerSample return "version - variantId"
                    continue;
                }
                this.Logger().Info($"YCABTestProvider, get variant ab-test, variant:= {variantId}");
                return variantId;
            }
            this.Logger().Error($"YCABTestProvider hasn't got ab-test variant, default ab-test variant:= {_controlVariant}, YCManager ab-test variant:= {YCManager.instance.abTestingManager.GetPlayerSample()}");
            return _controlVariant;
        }
        private bool IsVariantId(string variantId) => YCManager.instance.abTestingManager.IsPlayerSample(variantId); 
    }
}