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

        // Using dependency injection to inject the repositories to the service.
        public CheckoutService(ICheckoutRepository checkoutRepo, IProductsRepository productsRepo)
        {
            _checkoutRepo = checkoutRepo;
            _productsRepo = productsRepo;
        }

        public void AddItemToCheckout(int sku, decimal quantity, DiscountRule discountRule)
        {
            // First get the product with the corresponding SKU from repository 
            var product = _productsRepo.GetProduct(sku);

            if (product == null)
            {
                throw new ArgumentException($"Cannot find product with SKU : {sku}");
            }

            if (product.UnitPrice <= 0)
            {
                throw new ArgumentOutOfRangeException("Cannot add a product with invalid price");
            }

            if (quantity <= 0)
            {
                throw new ArgumentOutOfRangeException("Cannot add a product with negative or zero quantity");
            }

            if (product.MeasurmentUnit == MeasurmentUnits.UNIT && quantity != Math.Round(quantity))
            {
                throw new ArgumentException("Cannot add product sold by number with a floating decimal quantity");
            }

            // Finally add a "quantity" of this product to the list of 
            // checkout items if all the checking conditions are met
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
