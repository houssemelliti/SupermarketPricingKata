using System;
using Xunit;
using SupermarketPricingKata.Services;
using Moq;
using SupermarketPricingKata.Repositories;
using SupermarketPricingKata.Models;
using System.Collections.Generic;

namespace SupermarketPricingKataTest
{
    /// <summary>
    /// Test class for CheckoutService.
    /// </summary>
    public class CheckoutServiceTest
    {
        private readonly Mock<ICheckoutRepository> _checkoutRepoMock; // Mock object representing the CheckoutRepository
        private readonly Mock<IProductsRepository> _productsRepoMock; // Mock object representing the ProductsRepository
        private readonly List<CheckoutItem> _checkoutItems = new List<CheckoutItem>(); // A list of checkout items used by mock objects
        
        // A list containing examples of discount rules that can be applied
        private List<DiscountRule> discountRules = new List<DiscountRule>
        {
            new DiscountRule
            {
                Description = "Buy Three for a Dollar",
                Quantity = 3,
                Price = 1
            },
            new DiscountRule
            {
                Description = "Buy two, get one free",
                Quantity = 3,
                Price = 2
            },
            new DiscountRule
            {
                Description = "80% OFF",
                Quantity = 1
            }
        };

        /// <summary>
        /// Test class constructor.
        /// Used to initiate mock objects
        /// </summary>
        public CheckoutServiceTest()
        {
            // Initializing mock objects
            _checkoutRepoMock = new Mock<ICheckoutRepository>();
            _productsRepoMock = new Mock<IProductsRepository>();

            // Setting-up the CheckoutRepository mock object to imitate adding a CheckoutItem object to the list
            _checkoutRepoMock.Setup(r => r.AddItem(It.IsAny<Product>(), It.IsAny<decimal>(), It.IsAny<DiscountRule>()))
                .Callback<Product, decimal, DiscountRule>((product, quantity, rule) =>
                {
                    // using the provided parameters to the mock to create a CheckoutItem and add it to the list
                    product.DiscountRule = rule;
                    var checkoutItem = new CheckoutItem { Product = product, Quantity = quantity, Price = product.UnitPrice * quantity };
                    _checkoutItems.Add(checkoutItem);
                });

            // Setting-upp the CheckoutRepository mock for Remove operation
            _checkoutRepoMock.Setup(r => r.DeleteItem(It.IsAny<CheckoutItem>()))
                .Callback<CheckoutItem>((element) => _checkoutItems.Remove(element));

            // Setup CheckoutRepository mock to return the list of checkout items
            _checkoutRepoMock.Setup(r => r.GetCheckoutItems()).Returns(_checkoutItems);

            // Setting-up the ProductsRepository mock object to imitate getting an exapmle product with SKU = 1
            _productsRepoMock.Setup(r => r.GetProduct(1)).Returns(new Product
            {
                Sku = 1,
                Name = "Bread",
                UnitPrice = 0.4m,
                MeasurmentUnit = MeasurmentUnits.UNIT
            });

            // Setting-up the ProductsRepository mock object to imitate getting an exapmle product with SKU = 3
            _productsRepoMock.Setup(r => r.GetProduct(3)).Returns(new Product
            {
                Sku = 3,
                Name = "Apples",
                UnitPrice = 1.99m,
                MeasurmentUnit = MeasurmentUnits.POUND
            });

            // Setting-up the ProductsRepository mock object to imitate getting an exapmle product with SKU = 4
            _productsRepoMock.Setup(r => r.GetProduct(4)).Returns(new Product
            {
                Sku = 4,
                Name = "Milk",
                MeasurmentUnit = MeasurmentUnits.LITRE,
                UnitPrice = 1.25m
            });
        }

        /// <summary>
        /// Initiates the CheckoutService by dependency injection.
        /// </summary>
        /// <returns>An instance of the <see cref="CheckoutService"/>.</returns>
        private CheckoutService Subject()
        {
            return new CheckoutService(_checkoutRepoMock.Object, _productsRepoMock.Object);
        }

        /// <summary>
        /// Verify that we can add an item to the checkout.
        /// </summary>
        [Fact]
        public void Test_CanAddItemToCheckout()
        {
            var service = Subject();
            // Performing the service call to add 4 Bread items to the checkout
            service.AddItemToCheckout(1, 4, null);
        }

        /// <summary>
        /// Verify that ArgumentException is thrown when trying to add an item
        /// with SKU pointing to a non-existing product.
        /// </summary>
        [Fact]
        public void Test_ExceptionWhenInvalidSKU()
        {
            var service = Subject();
            // adding an item with SKU = 10 and expecting to get an exeption since there is no product with this SKU
            Assert.Throws<ArgumentException>(() => service.AddItemToCheckout(10, 1, null));
        }

        /// <summary>
        /// Verify that ArgumentOutOfRangeException is thrown when an item 
        /// with zero or negative price is added to the checkout.
        /// </summary>
        [Fact]
        public void Test_ExceptionWhenItemWithoutPrice()
        {
            var service = Subject();
            // Setup CheckoutRepository mock to add a product with zero price to the checkout
            _productsRepoMock.Setup(r => r.GetProduct(1)).Returns(new Product 
            { 
                Sku = 1, 
                UnitPrice = 0, 
                MeasurmentUnit = MeasurmentUnits.LITRE 
            });
            Assert.Throws<ArgumentOutOfRangeException>(() => service.AddItemToCheckout(1, 1, null));
        }

        /// <summary>
        /// Verify that ArgumentOutOfRangeException is thrown when checkout item
        /// is added with zero or negative quantity.
        /// </summary>
        [Fact]
        public void Test_ExceptionWhenQuantityIsNegativeOrZero()
        {
            var service = Subject();

            // Check exeption is thrown when adding an item with negative quantity
            Assert.Throws<ArgumentOutOfRangeException>(() => service.AddItemToCheckout(1, -3, null));
            
            // Check exeption is thrown when adding an item with zero quantity
            Assert.Throws<ArgumentOutOfRangeException>(() => service.AddItemToCheckout(1, 0, null));
        }

        /// <summary>
        /// Verify that ArgumentException is thrown when adding an item to the checkout
        /// where the item is sold by unit and the quantity is a floating decimal number.
        /// </summary>
        [Fact]
        public void Test_ExceptionWhenDecimalQuantityForProductSoldByNumber()
        {
            var service = Subject();
            // Testing with an item with SKU = 1 with measurment unit UNIT (item is sold by number) 
            // Try to add the item to the checkout with a 2.5 quantity
            Assert.Throws<ArgumentException>(() => service.AddItemToCheckout(1, 2.5m, null));
        }

        /// <summary>
        /// Verify that we can remove an item from the checkout.
        /// </summary>
        [Fact]
        public void Test_CanRemoveItemFromCheckout()
        {
            var service = Subject();

            // First add the item to the checkout
            service.AddItemToCheckout(3, 4, null);

            // Setting-up the CheckoutRepository mock object to imitate getting an exapmle checkoutItem
            var testItem = new CheckoutItem
            {
                Product = new Product { Sku = 3 },
                Quantity = 4
            };
            _checkoutRepoMock.Setup(r => r.GetCheckoutItem(3)).Returns(testItem);

            // Perform delete through the service
            service.DeleteItemFromCheckout(3);

            // Verify that the service called the repository mock object; otherwise MockException is thrown.
            // this is to verify that the service will call the real repository for removing the item.
            _checkoutRepoMock.Verify(r => r.DeleteItem(testItem)); 
        }

        /// <summary>
        /// Verify that the total is calculated properly 
        /// for a single item in checkout.
        /// </summary>
        [Fact]
        public void Test_CanCalculateTotalSingleItemPerNumber()
        {
            var service = Subject();
            service.AddItemToCheckout(1, 10, null); // adding 10 items with product SKU = 1 and unit price = $0.4
            Assert.Equal(4, service.CalculateTotal()); // verify that total checkout price is $4
        }

        /// <summary>
        /// Verify that total price is calculated properly
        /// for items sold per weight.
        /// </summary>
        [Fact]
        public void Test_CanCalculateTotalSingleItemPerWeight()
        {
            var service = Subject();

            // item with SKU = 3 costs $1.99/pound (so what does 4 ounces cost?)
            // 4 oz is equal to 0.25 lb
            service.AddItemToCheckout(3, 0.25m, null);

            // We expect 4 oz price to be equal to $0.5
            Assert.Equal(0.5m, service.CalculateTotal());
        }

        /// <summary>
        /// Verify that total price is calculated properly for 
        /// a checkout containing multiple items.
        /// </summary>
        [Fact]
        public void Test_CanCalculateCorrectTotalMultipleItems()
        {
            var service = Subject();

            service.AddItemToCheckout(1, 2, null); // 2 Bread items
            service.AddItemToCheckout(3, 0.5m, null); // 0,5 lb of Apples
            service.AddItemToCheckout(4, 3, null); // 3L of Milk

            Assert.Equal(5.54m, service.CalculateTotal());
        }

        /// <summary>
        /// Verify that a discount rule is applied properly on a checkout item.
        /// </summary>
        [Fact]
        public void Test_CanApplyDiscountRuleSingleItemPerNumber()
        {
            var service = Subject();

            // Adding 8 Bread items with unit price $0.4 to the checkout with rule "Three for a dollar"
            service.AddItemToCheckout(1, 8, discountRules[0]);
            // Verify that 6 bread items are subject to discount with rule "Three for a dollar"
            // and 2 items are excluded from discount
            Assert.Equal(2.8m, service.CalculateTotal());
        }

        /// <summary>
        /// Verify that different discount rules can be applied on multiple items
        /// regardless of the unit of measurment. Verify also that correct total
        /// price is calculated after applying discounts.
        /// </summary>
        [Fact]
        public void Test_CanApplyDiscountRulesMultipleItems()
        {
            var service = Subject();

            // Adding 4 Bread items to the checkout list with rule "Three for a dollar"
            service.AddItemToCheckout(1, 4, discountRules[0]);

            var apples = new Product
            {
                Sku = 3,
                Name = "Apples",
                MeasurmentUnit = MeasurmentUnits.POUND,
                UnitPrice = 1.99m
            };

            // Adding 5 Apples to the checkout list with rule "Buy two, get one free"
            _productsRepoMock.Setup(r => r.GetProduct(apples.Sku)).Returns(apples);

            var rule1 = discountRules[1];
            rule1.Price *= apples.UnitPrice; // set rule discount price depending on the product's unit price
            service.AddItemToCheckout(apples.Sku, 5, rule1);

            var milk = new Product
            {
                Sku = 4,
                Name = "Milk",
                MeasurmentUnit = MeasurmentUnits.LITRE,
                UnitPrice = 1.25m
            };

            // Adding 2L of Milk to the checkout list with rule "80% Off"
            _productsRepoMock.Setup(r => r.GetProduct(milk.Sku)).Returns(milk);

            var rule2 = discountRules[2];
            rule2.Price = milk.UnitPrice / 5;
            service.AddItemToCheckout(milk.Sku, 2, rule2);

            // Expect the discount to be applied on each product 
            // and total price to be calculated properly
            Assert.Equal(9.86m, service.CalculateTotal());
        }

        /// <summary>
        /// Verify that ArgumentOutOfRangeException is thrown when attepting  
        /// to add an item to the checkout with negative or zero discounted quantity.
        /// </summary>
        [Fact]
        public void Test_ExpetionWhenDiscountQuantityIsNegativeOrZero()
        {
            var service = Subject();
            
            var rule = discountRules[0]; // Get the rule "Three for a dollar" object 
            rule.Quantity = 0; // The discount quantity is set to zero
            service.AddItemToCheckout(1, 8, rule); // Adding 8 Bread items to the checkout with rule "Three for a dollar"

            Assert.Throws<ArgumentOutOfRangeException>(() => service.CalculateTotal());
        }
    }
}
