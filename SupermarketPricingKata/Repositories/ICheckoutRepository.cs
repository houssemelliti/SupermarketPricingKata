using SupermarketPricingKata.Models;
using System.Collections.Generic;

namespace SupermarketPricingKata.Repositories
{
    /// <summary>
    /// Interfaces with the checkout data store.
    /// </summary>
    public interface ICheckoutRepository
    {
        /// <summary>
        /// Gets the list of all checkout items.
        /// </summary>
        IList<CheckoutItem> GetCheckoutItems();

        /// <summary>
        /// Gets a checkout item based on the product SKU.
        /// </summary>
        /// <param name="sku">Product identifier or stock keeping unit.</param>
        /// <returns><see cref="CheckoutItem"/> Object.</returns>
        CheckoutItem GetCheckoutItem(int sku);

        /// <summary>
        /// Adds an item to the checkout list.
        /// </summary>
        /// <param name="product">The product to add to the checkout.</param>
        /// <param name="quantity">The number, weight or volume of the product to be added to the checkout.</param>
        /// <param name="discountRule">The discount rule to be applied on the product. This parameter can be null.</param>
        void AddItem(Product product, decimal quantity, DiscountRule discountRule);

        /// <summary>
        /// Deletes a checkout item from the checkout list.
        /// </summary>
        /// <param name="item">Checkout Item to be removed.</param>
        /// <returns>
        /// true if <paramref name="item"/> was successfully deleted;
        /// otherwise, false. 
        /// </returns>
        bool DeleteItem(CheckoutItem item);
    }
}
