using SupermarketPricingKata.Models;
using System;
using System.Collections.Generic;

namespace SupermarketPricingKata.Services
{
    public class CheckoutService : ICheckoutService
    {
        public void AddItemToCheckout(int sku, decimal quantity, DiscountRule discountRule)
        {
            throw new NotImplementedException();
        }

        public bool DeleteItemFromCheckout(int sku)
        {
            throw new NotImplementedException();
        }

        public decimal CalculateTotal()
        {
            throw new NotImplementedException();
        }

        public IList<CheckoutItem> GetCheckoutItems()
        {
            throw new NotImplementedException();
        }
    }
}
