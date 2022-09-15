namespace Dino.ABTest
{
    public static class ABTestExt
    {
        public static bool WithoutAmmo(this Feofun.ABTest.ABTest abTest) =>
                abTest.CurrentVariantId.Equals(ABTestVariantId.WithoutAmmo.ToCamelCase());
    }
}