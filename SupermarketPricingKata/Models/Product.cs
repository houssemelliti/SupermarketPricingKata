namespace SupermarketPricingKata.Models
{
    /// <summary>
    /// Represents the list of measurment units for products.
    /// </summary>
    public enum MeasurmentUnits { UNIT, POUND, LITRE, METRE, GRAM }

    /// <summary>
    /// Models a product object in the store.
    /// </summary>
    public class Product
    {
        /// <summary>
        /// Stock Keeping Unit of the product.
        /// </summary>
        public int Sku { get; set; }
        
        /// <summary>
        /// Name of the product.
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Product's unit price
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// Product measurment unit (per item, weight, volume, etc.).
        /// </summary>
        public MeasurmentUnits MeasurmentUnit { get; set; }

        /// <summary>
        /// Discount rule to be applied on the product.
        /// </summary>
        public DiscountRule DiscountRule { get; set; }
    }
}
