using JetBrains.Annotations;
using Logger.Extension;
using UnityEngine;
using Zenject;

namespace Feofun.ABTest.Providers
{
    public class OverrideABTestProvider : IABTestProvider
    {
        private const string OVERRIDE_AB_TEST_KEY = "OverrideAbTestId";

        private readonly string _controlVariant;
        private readonly IABTestProvider _impl;

        [Inject]
        private IABTestCheatManager _cheatsManager;

        public OverrideABTestProvider([CanBeNull] IABTestProvider impl, string controlVariant)
        {
            _impl = impl;
            _controlVariant = controlVariant;
        }
        public string GetVariant() => _cheatsManager.IsABTestCheatEnabled ? GetOverrideVariant() : ImplVariant;

        private string ImplVariant => _impl?.GetVariant() ?? _controlVariant;

        private string GetOverrideVariant()
        {
            var variantId = GetVariantFromPlayerPrefs();
            this.Logger().Info($"OverrideABTestProvider, get variant ab-test, variant:= {variantId}"); 
            return variantId;
        }
        public static void SetVariantId(string variantId) => PlayerPrefs.SetString(OVERRIDE_AB_TEST_KEY, variantId);
        private string GetVariantFromPlayerPrefs() => PlayerPrefs.GetString(OVERRIDE_AB_TEST_KEY, _controlVariant);
    
    }
}
