using SupermarketPricingKata.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SupermarketPricingKata.Repositories
{
    public class CheckoutRepository: ICheckoutRepository
    {
        // This could be replaced by a database connection
        private static IList<CheckoutItem> _checkoutItems = new List<CheckoutItem>();
        
        public void AddItem(Product product, decimal quantity, DiscountRule discountRule)
        {
            // create a CheckoutItem object from the provided parameters
            var checkoutItem = new CheckoutItem { Product = product, Quantity = quantity, Price = product.UnitPrice * quantity };

            // add the item to the checkout list
            _checkoutItems.Add(checkoutItem);
        }

        public bool DeleteItem(CheckoutItem item)
        {
            return _checkoutItems.Remove(item);
        }

        public CheckoutItem GetCheckoutItem(int sku)
        {
            return _checkoutItems.SingleOrDefault(c => c.Product.Sku == sku);
        }

        public IList<CheckoutItem> GetCheckoutItems()
        {
            return _checkoutItems.ToList();
        }
    }
}
