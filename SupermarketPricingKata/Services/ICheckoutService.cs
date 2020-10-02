using SupermarketPricingKata.Models;
using System.Collections.Generic;

namespace SupermarketPricingKata.Services
{
    /// <summary>
    /// Holds the business logic for checkout management.
    /// </summary>
    public interface ICheckoutService
    {
        /// <summary>
        /// Adds an item to the checkout.
        /// </summary>
        /// <param name="sku">Item identifier.</param>
        /// <param name="quantity">Number of units to be added.</param>
        /// <param name="discountRule">Discount rule to be applied on the item.</param>
        void AddItemToCheckout(int sku, decimal quantity, DiscountRule discountRule);

        /// <summary>
        /// Deletes an item from the checkout.
        /// </summary>
        /// <param name="id">Item identifier in the checkout list.</param>
        /// <returns>True if the item was removed successfully; otherwise returns False.</returns>
        bool DeleteItemFromCheckout(int id);

        /// <summary>
        /// Gets the list of available discount rules from repository.
        /// </summary>
        /// <returns>The list of dicount rules.</returns>
        IList<DiscountRule> GetDiscountRules();

        /// <summary>
        /// Gets a discount rule from repository based on its identifier.
        /// </summary>
        /// <param name="id">The discount rule identifier.</param>
        /// <returns>A <see cref="DiscountRule"/> object.</returns>
        DiscountRule GetDiscountRule(int id);

        /// <summary>
        /// Calculates the total price value of the checkout.
        /// </summary>
        /// <returns>A decimal value representing the total price rounded to 2 decimals.</returns>
        decimal CalculateTotal();
        
        /// <summary>
        /// Gets the list of all checkout items.
        /// </summary>
        /// <returns>A list of checkout items.</returns>
        IList<CheckoutItem> GetCheckoutItems();
    }
}
