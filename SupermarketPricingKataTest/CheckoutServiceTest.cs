using System;
using Xunit;
using SupermarketPricingKata.Services;

namespace SupermarketPricingKataTest
{
    /// <summary>
    /// Test class for CheckoutService
    /// </summary>
    public class CheckoutServiceTest
    {
        /// <summary>
        /// Verify that we can add an item to the checkout
        /// </summary>
        [Fact]
        public void Test_CanAddItemToCheckout()
        {
            var service = new CheckoutService();
            service.AddItemToCheckout(1, 4, null);
        }
    }
}
