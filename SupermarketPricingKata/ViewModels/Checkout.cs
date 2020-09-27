using SupermarketPricingKata.Models;
using System.Collections.Generic;

namespace SupermarketPricingKata.ViewModels
{
    /// <summary>
    /// View model representing the checkout object.
    /// </summary>
    public class Checkout
    {
        /// <summary>
        /// Models the list of items in the checkout.
        /// </summary>
        public IList<CheckoutItem> CheckoutItems { get; set; }
        
        /// <summary>
        /// Models the total price of the checkout.
        /// </summary>
        public decimal TotalPrice { get; set; }
    }
}
