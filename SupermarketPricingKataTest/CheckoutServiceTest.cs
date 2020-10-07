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
        private readonly Mock<IDiscountsRepository> _discountsRepoMock; // Mock object representing the DiscountsRepository
        private readonly List<CheckoutItem> _checkoutItems = new List<CheckoutItem>(); // A list of checkout items used by mock objects

        /// <summary>
        /// Test class constructor.
        /// Used to initiate mock objects
        /// </summary>
        public CheckoutServiceTest()
        {
            // Initializing mock objects
            _checkoutRepoMock = new Mock<ICheckoutRepository>();
            _productsRepoMock = new Mock<IProductsRepository>();
            _discountsRepoMock = new Mock<IDiscountsRepository>();

            // Setuo repository mock objects
            SetupMockObjects();
        }

        /// <summary>
        /// Initiates the CheckoutService by dependency injection.
        /// </summary>
        /// <returns>An instance of the <see cref="CheckoutService"/>.</returns>
        private CheckoutService Subject()
        {
            return new CheckoutService(_checkoutRepoMock.Object, _productsRepoMock.Object, _discountsRepoMock.Object);
        }

        /// <summary>
        /// Verify that we can add an item to the checkout.
        /// </summary>
        [Fact]
        public void Test_CanAddItemToCheckout()
        {
            var checkoutService = Subject();
            // Performing the checkoutService call to add 4 Bread items to the checkout
            checkoutService.AddItemToCheckout(1, 4, MeasurmentUnits.PIECE, null);
        }

        /// <summary>
        /// Verify that ArgumentException is thrown when trying to add an item
        /// with SKU pointing to a non-existing product.
        /// </summary>
        [Fact]
        public void Test_ExceptionWhenInvalidSKU()
        {
            var checkoutService = Subject();
            // adding an item with SKU = 10 and expecting to get an exeption since there is no product with this SKU
            Assert.Throws<ArgumentException>(() => checkoutService.AddItemToCheckout(10, 1, MeasurmentUnits.PIECE, null));
        }

        /// <summary>
        /// Verify that ArgumentOutOfRangeException is thrown when an item 
        /// with zero or negative price is added to the checkout.
        /// </summary>
        [Fact]
        public void Test_ExceptionWhenItemWithoutPrice()
        {
            var checkoutService = Subject();
            // Setup CheckoutRepository mock to add a product with zero price to the checkout
            _productsRepoMock.Setup(r => r.GetProduct(1)).Returns(new Product 
            { 
                Sku = 1, 
                UnitPrice = 0, 
                MeasurmentUnit = MeasurmentUnits.LITRE 
            });
            Assert.Throws<ArgumentOutOfRangeException>(() => checkoutService.AddItemToCheckout(1, 1, MeasurmentUnits.PIECE, null));
        }

        /// <summary>
        /// Verify that ArgumentOutOfRangeException is thrown when checkout item
        /// is added with zero or negative quantity.
        /// </summary>
        [Fact]
        public void Test_ExceptionWhenQuantityIsNegativeOrZero()
        {
            var checkoutService = Subject();

            // Check exeption is thrown when adding an item with negative quantity
            Assert.Throws<ArgumentOutOfRangeException>(() => checkoutService.AddItemToCheckout(1, -3, MeasurmentUnits.PIECE, null));
            
            // Check exeption is thrown when adding an item with zero quantity
            Assert.Throws<ArgumentOutOfRangeException>(() => checkoutService.AddItemToCheckout(1, 0, MeasurmentUnits.PIECE, null));
        }

        /// <summary>
        /// Verify that ArgumentException is thrown when adding an item to the checkout
        /// where the item is sold by unit and the quantity is a floating decimal number.
        /// </summary>
        [Fact]
        public void Test_ExceptionWhenDecimalQuantityForProductSoldByNumber()
        {
            var checkoutService = Subject();
            // Testing with an item with SKU = 1 with measurment unit UNIT (item is sold by number) 
            // Try to add the item to the checkout with a 2.5 quantity
            Assert.Throws<ArgumentException>(() => checkoutService.AddItemToCheckout(1, 2.5m, MeasurmentUnits.PIECE, null));
        }

        /// <summary>
        /// Verify that we can remove an item from the checkout.
        /// </summary>
        [Fact]
        public void Test_CanRemoveItemFromCheckout()
        {
            var checkoutService = Subject();

            // First add the item to the checkout
            checkoutService.AddItemToCheckout(3, 4, MeasurmentUnits.POUND, null);

            // Setting-up the CheckoutRepository mock object to imitate getting an exapmle checkoutItem
            var testItem = new CheckoutItem
            {
                Id = 1,
                Product = new Product { Sku = 3 },
                Quantity = 4
            };
            _checkoutRepoMock.Setup(r => r.GetCheckoutItem(1)).Returns(testItem);

            // Perform delete through the checkoutService
            checkoutService.DeleteItemFromCheckout(1);

            // Verify that the checkoutService called the repository mock object; otherwise MockException is thrown.
            // this is to verify that the checkoutService will call the real repository for removing the item.
            _checkoutRepoMock.Verify(r => r.DeleteItem(testItem)); 
        }

        /// <summary>
        /// Verify that the total is calculated properly 
        /// for a single item in checkout.
        /// </summary>
        [Fact]
        public void Test_CanCalculateTotalSingleItemPerNumber()
        {
            var checkoutService = Subject();
            checkoutService.AddItemToCheckout(1, 10, MeasurmentUnits.PIECE, null); // adding 10 items with product SKU = 1 and unit price = $0.4
            Assert.Equal(4, checkoutService.CalculateTotal()); // verify that total checkout price is $4
        }

        /// <summary>
        /// Verify that total price is calculated properly
        /// for items sold per weight.
        /// </summary>
        [Fact]
        public void Test_CanCalculateTotalSingleItemPerWeight()
        {
            var checkoutService = Subject();

            // item with SKU = 3 costs $1.99/pound (so what does 4 ounces cost?)
            // setting buy unit to ounces while sale unit is pound
            checkoutService.AddItemToCheckout(3, 4, MeasurmentUnits.OUNCE, null);

            // We expect 4 oz price to be equal to $0.5
            Assert.Equal(0.5m, checkoutService.CalculateTotal());
        }

        /// <summary>
        /// Verify that ArgumentException is thrown when buy unit
        /// is in a different category than sell unit.
        /// </summary>
        [Fact]
        public void Test_ExceptionWhenIncompatibleUnits()
        {
            var checkoutService = Subject();

            // item with SKU = 3 is sold per pound
            // setting buy unit to litres while sale unit is pound and verify that exception is thrown
            Assert.Throws<ArgumentException>(() => checkoutService.AddItemToCheckout(3, 4, MeasurmentUnits.LITRE, null));
        }

        /// <summary>
        /// Verify that total price is calculated properly for 
        /// a checkout containing multiple items.
        /// </summary>
        [Fact]
        public void Test_CanCalculateCorrectTotalMultipleItems()
        {
            var checkoutService = Subject();

            checkoutService.AddItemToCheckout(1, 2, MeasurmentUnits.PIECE, null); // 2 Bread items
            checkoutService.AddItemToCheckout(3, 0.5m, MeasurmentUnits.POUND, null); // 0,5 lb of Apples
            checkoutService.AddItemToCheckout(4, 3, MeasurmentUnits.LITRE, null); // 3L of Milk

            Assert.Equal(5.54m, checkoutService.CalculateTotal());
        }

        /// <summary>
        /// Verify that a discount rule is applied properly on a checkout item.
        /// </summary>
        [Fact]
        public void Test_CanApplyDiscountRuleSingleItemPerNumber()
        {
            var checkoutService = Subject();
            
            // Adding 8 Bread items with unit price $0.4 to the checkout with rule "Three for a dollar"
            checkoutService.AddItemToCheckout(1, 8, MeasurmentUnits.PIECE, checkoutService.GetDiscountRule(1));
            // Verify that 6 bread items are subject to discount with rule "Three for a dollar"
            // and 2 items are excluded from discount
            Assert.Equal(2.8m, checkoutService.CalculateTotal());
        }

        /// <summary>
        /// Verify that different discount rules can be applied on multiple items
        /// regardless of the unit of measurment. Verify also that correct total
        /// price is calculated after applying discounts.
        /// </summary>
        [Fact]
        public void Test_CanApplyDiscountRulesMultipleItems()
        {
            var checkoutService = Subject();

            // Adding 4 Bread items to the checkout list with rule "Three for a dollar"
            checkoutService.AddItemToCheckout(1, 4, MeasurmentUnits.PIECE, checkoutService.GetDiscountRule(1));

            // Adding 5 lb of Apple to the checkout list with rule "Buy two, get one free"
            checkoutService.AddItemToCheckout(3, 5, MeasurmentUnits.POUND, checkoutService.GetDiscountRule(2));

            // Adding 2.5L of Milk to the checkout list with rule "80% Off"
            checkoutService.AddItemToCheckout(4, 2.5m, MeasurmentUnits.LITRE, checkoutService.GetDiscountRule(3));

            // Expect the discount to be applied on each product 
            // and total price to be calculated properly
            Assert.Equal(9.98m, checkoutService.CalculateTotal());
        }

        /// <summary>
        /// Verify that ArgumentOutOfRangeException is thrown when attempting  
        /// to add an item to the checkout with negative or zero discounted quantity.
        /// </summary>
        [Fact]
        public void Test_ExpetionWhenDiscountQuantityIsNegativeOrZero()
        {
            var checkoutService = Subject();
            
            var rule = checkoutService.GetDiscountRule(1); // Get the rule "Three for a dollar" object 
            rule.Quantity = 0; // The discount quantity is set to zero
            checkoutService.AddItemToCheckout(1, 8, MeasurmentUnits.PIECE, rule); // Adding 8 Bread items to the checkout with rule "Three for a dollar"

            // Expect an exception to be thrown
            Assert.Throws<ArgumentOutOfRangeException>(() => checkoutService.CalculateTotal());
        }

        /// <summary>
        /// Verify that ArgumentOutOfRangeException is thrown when attempting
        /// to add a checkout item with negative discount price
        /// </summary>
        [Fact]
        public void Test_ExpetionWhenDiscountPriceIsNegative()
        {
            var checkoutService = Subject();

            // Adding 8 Bread items to the checkout
            // The discount rule price is negative
            checkoutService.AddItemToCheckout(1, 8, MeasurmentUnits.PIECE, checkoutService.GetDiscountRule(5));

            // Expect an exception to be thrown
            Assert.Throws<ArgumentOutOfRangeException>(() => checkoutService.CalculateTotal());
        }

        /// <summary>
        /// Used to set-up the repository mock objects for test class.
        /// </summary>
        private void SetupMockObjects()
        {
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
                MeasurmentUnit = MeasurmentUnits.PIECE
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

            // Setting-up the DiscountsRepository mock object to imitate getting an exapmle discount rule with id = 1
            _discountsRepoMock.Setup(r => r.GetDiscountRule(1)).Returns(new DiscountRule
            {
                Id = 1,
                Description = "Buy Three for a Dollar",
                Quantity = 3
            });

            // Setting-up the DiscountsRepository mock object to imitate getting an exapmle discount rule with id = 2
            _discountsRepoMock.Setup(r => r.GetDiscountRule(2)).Returns(new DiscountRule
            {
                Id = 2,
                Description = "Buy two, get one free",
                Quantity = 3
            });

            // Setting-up the DiscountsRepository mock object to imitate getting an exapmle discount rule with id = 3
            _discountsRepoMock.Setup(r => r.GetDiscountRule(3)).Returns(new DiscountRule
            {
                Id = 3,
                Description = "80% OFF",
                Quantity = 1
            });

            // Setting-up the DiscountsRepository mock object to imitate getting an exapmle discount rule negative price
            _discountsRepoMock.Setup(r => r.GetDiscountRule(5)).Returns(new DiscountRule
            {
                Id = 5,
                Quantity = 1,
                Price = -1 // The discount price is set to a negative value
            });
        }
    }
}
