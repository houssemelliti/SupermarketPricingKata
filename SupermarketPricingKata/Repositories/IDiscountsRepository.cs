using SupermarketPricingKata.Models;
using System.Collections.Generic;

namespace SupermarketPricingKata.Repositories
{
    /// <summary>
    /// Interfaces with the discount rules data store.
    /// </summary>
    public interface IDiscountsRepository
    {
        /// <summary>
        /// Gets the list of available discount rules.
        /// </summary>
        /// <returns>The list of dicount rules.</returns>
        IList<DiscountRule> GetDiscountRules();
        
        /// <summary>
        /// Gets a discount rule based on its identifier.
        /// </summary>
        /// <param name="id">The discount rule identifier.</param>
        /// <returns>A <see cref="DiscountRule"/> object.</returns>
        DiscountRule GetDiscountRule(int id);
    }
}
