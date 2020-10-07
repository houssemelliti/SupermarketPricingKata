namespace SupermarketPricingKata.Commons
{
    /// <summary>
    /// Helper class to convert mass units.
    /// </summary>
    public static class MassConversion
    {
        public static decimal GramsToKilograms(decimal grams)
        {
            return grams * .001m;
        }
        public static decimal GramsToPounds(decimal grams)
        {
            return grams * .00220462m;
        }
        public static decimal GramsToOunces(decimal grams)
        {
            return grams * .035274m;
        }
        public static decimal KilogramsToGrams(decimal kilograms)
        {
            return kilograms * 1000;
        }
        public static decimal KilogramsToPounds(decimal kilograms)
        {
            return kilograms * 2.20462m;
        }
        public static decimal KilogramsToOunces(decimal kilograms)
        {
            return kilograms * 35.274m;
        }
        public static decimal PoundsToGrams(decimal pounds)
        {
            return pounds * 453.592m;
        }
        public static decimal PoundsToKilograms(decimal pounds)
        {
            return pounds * .453592m;
        }
        public static decimal PoundsToOunces(decimal pounds)
        {
            return pounds * 16;
        }
        public static decimal OuncesToGrams(decimal ounces)
        {
            return ounces * 28.3495m;
        }
        public static decimal OuncesToKilograms(decimal ounces)
        {
            return ounces * .0283495m;
        }
        public static decimal OuncesToPounds(decimal ounces)
        {
            return ounces * .0625m;
        }
    }
}
