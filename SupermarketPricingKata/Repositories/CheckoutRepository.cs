using SupermarketPricingKata.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SupermarketPricingKata.Repositories
{
    public class CheckoutRepository: ICheckoutRepository
    {
        // This could be replaced by a database connection
        private static IList<CheckoutItem> _checkoutItems = new List<CheckoutItem>();
        private static int _id = 0;
        public void AddItem(Product product, decimal quantity, DiscountRule discountRule)
        {
            product.DiscountRule = discountRule;

            // create a CheckoutItem object from the provided parameters
            var checkoutItem = new CheckoutItem { Product = product, Quantity = quantity, Price = product.UnitPrice * quantity };

            // Set a unique ID and increment the static counter
            checkoutItem.Id = _id;
            Interlocked.Increment(ref _id);

            // add the item to the checkout list
            _checkoutItems.Add(checkoutItem);
        }

        public bool DeleteItem(CheckoutItem item)
        {
            return _checkoutItems.Remove(item);
        }

        public CheckoutItem GetCheckoutItem(int id)
        {
            return _checkoutItems.SingleOrDefault(c => c.Id == id);
        }

        public IList<CheckoutItem> GetCheckoutItems()
        {
            return _checkoutItems.ToList();
        }
    }
}
