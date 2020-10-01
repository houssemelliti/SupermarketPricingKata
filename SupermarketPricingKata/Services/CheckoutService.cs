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

        public IList<CheckoutItem> GetCheckoutItems()
        {
            return _checkoutRepo.GetCheckoutItems();
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

        public bool DeleteItemFromCheckout(int id)
        {
            var item = _checkoutRepo.GetCheckoutItem(id); // Get the item by Id from repository
            return _checkoutRepo.DeleteItem(item);
        }

        public decimal CalculateTotal()
        {
            IList<CheckoutItem> checkoutItems = _checkoutRepo.GetCheckoutItems();

            var totalPrice = 0m;

            // Apply price calculation for every item in the checkout
            foreach (var item in checkoutItems)
            {
                if (item.Product.DiscountRule != null)
                {
                    totalPrice += CalculateTotalForItemWithDiscount(item);
                }
                else // the item has no discounts to be applied
                {
                    totalPrice += item.Price;
                }
            }
            // Prices are rounded to 2 decimals
            return Math.Round(totalPrice, 2);
        }

        private decimal CalculateTotalForItemWithDiscount(CheckoutItem item)
        {
            if (item.Product.DiscountRule.Quantity <= 0)
            {
                throw new ArgumentOutOfRangeException("Cannot add a discount with negative or zero quantity");
            }

            if (item.Product.DiscountRule.Price < 0)
            {
                throw new ArgumentOutOfRangeException("Cannot add a discount with negative price");
            }

            var totalPrice = 0m;
            
            // calculate the number of items subject to discount based on the discount rule's number of discounts
            var nbrOfDiscounts = Math.Floor(item.Quantity / item.Product.DiscountRule.Quantity);
            totalPrice += nbrOfDiscounts * item.Product.DiscountRule.Price;
            
            // calculate the number of elements excluded from discount
            var remaining = item.Quantity - nbrOfDiscounts * item.Product.DiscountRule.Quantity;
            totalPrice += remaining * item.Product.UnitPrice;
            
            return totalPrice;
        }
    }
}
