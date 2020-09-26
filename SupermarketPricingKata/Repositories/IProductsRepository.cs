using SupermarketPricingKata.Models;
using System.Collections.Generic;

namespace SupermarketPricingKata.Repositories
{
    /// <summary>
    /// Interfaces with the products data store.
    /// </summary>
    public interface IProductsRepository
    {
        /// <summary>
        /// Gets the list of available products in the data store.
        /// </summary>
        /// <returns>A list of <see cref="Product"/>.</returns>
        IList<Product> GetProducts();

        /// <summary>
        /// Gets a single <see cref="Product"/> based on its SKU.
        /// </summary>
        /// <param name="sku">Stock Keeping Unit of the product.</param>
        /// <returns>A <see cref="Product"/>.</returns>
        Product GetProduct(int sku);
    }
}
