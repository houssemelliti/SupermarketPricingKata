using SupermarketPricingKata.Models;
using System.Collections.Generic;
using System.Linq;

namespace SupermarketPricingKata.Repositories
{
    public class ProductsRepository: IProductsRepository
    {
        // Predefined list of products used as an example
        // This could be later replaced by a database connection
        private readonly IList<Product> _productsList = new List<Product>
        {
            new Product
            {
                Sku = 1,
                Name = "Bread",
                MeasurmentUnit = MeasurmentUnits.UNIT,
                UnitPrice = 0.4m
            },
            new Product
            {
                Sku = 2,
                Name = "Eggs",
                MeasurmentUnit = MeasurmentUnits.UNIT,
                UnitPrice = 1
            },
            new Product
            {
                Sku = 3,
                Name = "Apples",
                MeasurmentUnit = MeasurmentUnits.POUND,
                UnitPrice = 1.99m
            },
            new Product
            {
                Sku = 4,
                Name = "Milk",
                MeasurmentUnit = MeasurmentUnits.LITRE,
                UnitPrice = 1.25m
            },
            new Product
            {
                Sku = 5,
                Name = "Bananas",
                MeasurmentUnit = MeasurmentUnits.POUND,
                UnitPrice = 3.8m
            }
        };
        public IList<Product> GetProducts()
        {
            return _productsList.ToList();
        }

        public Product GetProduct(int sku)
        {
            return _productsList.SingleOrDefault(product => product.Sku == sku);
        }
    }
}
