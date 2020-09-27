using SupermarketPricingKata.Models;
using System.Collections.Generic;

namespace SupermarketPricingKata.Services
{
    /// <summary>
    /// Holds the business logic related to products management.
    /// </summary>
    public interface IProductsService
    {
        /// <summary>
        /// Gets the list of all available products in the store.
        /// </summary>
        /// <returns>A list of products.</returns>
        IList<Product> GetAllProducts();
        
        /// <summary>
        /// Gets a single <see cref="Product"/> based on its <paramref name="sku"/>.
        /// </summary>
        /// <param name="sku">Stock keeping unit or product identifier.</param>
        /// <returns>A <see cref="Product"/> object.</returns>
        Product GetProduct(int sku);
    }
}
