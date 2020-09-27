using SupermarketPricingKata.Models;
using SupermarketPricingKata.Repositories;
using System.Collections.Generic;

namespace SupermarketPricingKata.Services
{
    public class ProductsService: IProductsService
    {
        private readonly IProductsRepository _productsRepo;
        public ProductsService(IProductsRepository productsRepo)
        {
            // Inject the products repository instance to the service
            _productsRepo = productsRepo;
        }
        public IList<Product> GetAllProducts()
        {
            return _productsRepo.GetProducts();
        }
        public Product GetProduct(int sku)
        {
            return _productsRepo.GetProduct(sku);
        }
    }
}
