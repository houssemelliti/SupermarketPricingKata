using SupermarketPricingKata.Models;

namespace SupermarketPricingKata.ViewModels
{
    /// <summary>
    /// Models a checkout item with integrated discount rule.
    /// </summary>
    public class DiscountedCheckoutItem
    {
        /// <summary>
        /// Models the product object in the item.
        /// </summary>
        public Product Product { get; set; }
        
        /// <summary>
        /// Models the quantity of <see cref="Models.Product"/> to be added to checkout.
        /// </summary>
        public decimal Quantity { get; set; }

        /// <summary>
        /// Models the <see cref="Models.DiscountRule"/> object associated with the <see cref="Models.Product"/>.
        /// </summary>
        public DiscountRule DiscountRule { get; set; }
    }
}
