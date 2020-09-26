namespace SupermarketPricingKata.Models
{
    /// <summary>
    /// Models a checkout item.
    /// </summary>
    public class CheckoutItem
    {
        /// <summary>
        /// Models the <see cref="Models.Product"/> object in the checkout item.
        /// </summary>
        public Product Product { get; set; }
        
        /// <summary>
        /// Quantity of product to be added.
        /// </summary>
        public decimal Quantity { get; set; }
        
        /// <summary>
        /// Total price for the checkout item.
        /// </summary>
        public decimal Price { get; set; }
    }
}
