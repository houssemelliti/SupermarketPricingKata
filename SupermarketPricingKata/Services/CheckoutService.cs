using SupermarketPricingKata.Models;
using SupermarketPricingKata.Repositories;
using System;
using System.Collections.Generic;

namespace SupermarketPricingKata.Services
{
    public class CheckoutService : ICheckoutService
    {
        private readonly ICheckoutRepository _checkoutRepo;
        private readonly IProductsRepository _productsRepo;

        public CheckoutService(ICheckoutRepository checkoutRepo, IProductsRepository itemsRepo)
        {
            _checkoutRepo = checkoutRepo;
            _productsRepo = itemsRepo;
        }

        public void AddItemToCheckout(int sku, decimal quantity, DiscountRule discountRule)
        {
            // First get the product with the corresponding SKU from repository 
            var product = _productsRepo.GetProduct(sku);
            
            // Then add a "quantity" of this product to the list of checkout items
            _checkoutRepo.AddItem(product, quantity, discountRule);
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
