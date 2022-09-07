namespace Dino.ABTest
{
    public static class ABTestExt
    {
        public static bool WithDisasters(this Feofun.ABTest.ABTest abTest) => 
            abTest.CurrentVariantId.Equals(ABTestVariantId.WithDisasters.ToCamelCase());
    }
}