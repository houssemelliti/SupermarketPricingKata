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
        private readonly IDiscountsRepository _discountsRepo;

        // Using dependency injection to inject the repositories to the service.
        public CheckoutService(ICheckoutRepository checkoutRepo, IProductsRepository productsRepo, IDiscountsRepository discountsRepo)
        {
            _checkoutRepo = checkoutRepo;
            _productsRepo = productsRepo;
            _discountsRepo = discountsRepo;
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

            if(discountRule != null)
            {
                discountRule = ConfigureDiscountRuleParams(discountRule.Id, product.UnitPrice);
                
            }

            // Finally add a "quantity" of this product to the list of 
            // checkout items if all the checking conditions are met
            _checkoutRepo.AddItem(product, quantity, discountRule);
        }

        public DiscountRule GetDiscountRule(int id)
        {
            return _discountsRepo.GetDiscountRule(id);
        }

        public IList<DiscountRule> GetDiscountRules()
        {
            return _discountsRepo.GetDiscountRules();
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

            if (item.Product.DiscountRule.Quantity == 1)
            {
                // In this case, the discount is applied on the total number or quantity of products
                totalPrice += item.Quantity * item.Product.DiscountRule.Price;
            }
            else
            {
                // calculate the number of items subject to discount based on the discount rule's number of discounts
                var nbrOfDiscounts = Math.Floor(item.Quantity / item.Product.DiscountRule.Quantity);
                totalPrice += nbrOfDiscounts * item.Product.DiscountRule.Price;

                // calculate the number of elements excluded from discount
                var remaining = item.Quantity - nbrOfDiscounts * item.Product.DiscountRule.Quantity;
                totalPrice += remaining * item.Product.UnitPrice;
            }
            
            return totalPrice;
        }

        private DiscountRule ConfigureDiscountRuleParams(int id, decimal unitPrice)
        {
            var discountRule = GetDiscountRule(id);
            switch(id)
            {
                case 1: // Rule "Buy Three for a Dollar"
                    discountRule.Price = 1;
                    break;
                case 2: // Rule "Buy two, get one free"
                    discountRule.Price = unitPrice * 2;
                    break;
                case 3: // Rule "80% Off"
                    discountRule.Price = unitPrice / 5;
                    break;
                case 4: // Rule "50% Off"
                    discountRule.Price = unitPrice / 2;
                    break;

                /* You can add as many cases as you want following this pattern */

                default:
                    break;
            }
            return discountRule;
        }
    }
}
