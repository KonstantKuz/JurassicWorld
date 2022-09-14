namespace Dino.ABTest
{
    public enum ABTestVariantId
    {
        Control,
        WithoutAmmo
        
    }
    public static class ABTestIdExtension
    {
        public static string ToCamelCase(this ABTestVariantId abTestVariantId)
        {
            var variantId = abTestVariantId.ToString();
            return char.ToLower(variantId[0]) + variantId.Substring(1);
        }
    }
    
}