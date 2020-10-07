namespace SupermarketPricingKata.Models
{
    /// <summary>
    /// Models a checkout item.
    /// </summary>
    public class CheckoutItem
    {
        /// <summary>
        /// The item identifier in the checkout list.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Models the <see cref="Models.Product"/> object in the checkout item.
        /// </summary>
        public Product Product { get; set; }
        
        /// <summary>
        /// Quantity of product to be added.
        /// </summary>
        public decimal Quantity { get; set; }

        /// <summary>
        /// Models the buy measurment unit. 
        /// </summary>
        public MeasurmentUnits BuyUnit { get; set; }

        /// <summary>
        /// Total price for the checkout item.
        /// </summary>
        public decimal Price { get; set; }
    }
}
