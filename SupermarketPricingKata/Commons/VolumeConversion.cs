namespace SupermarketPricingKata.Commons
{
    /// <summary>
    /// Helper class to convert volume units.
    /// </summary>
    public static class VolumeConversion
    {
        public static decimal LitresToMillilitres(decimal litres)
        {
            return litres * 1000;
        }
        public static decimal LitresToGallons(decimal litres)
        {
            return litres * .264172m;
        }
        public static decimal MillilitresToLitres(decimal millilitres)
        {
            return millilitres * .001m;
        }
        public static decimal MillilitresToGallons(decimal millilitres)
        {
            return millilitres * .000264172m;
        }
        public static decimal GallonsToLitres(decimal gallons)
        {
            return gallons * 3.78541m;
        }
        public static decimal GallonsToMillilitres(decimal gallons)
        {
            return gallons * 3785.41m;
        }
    }
}
