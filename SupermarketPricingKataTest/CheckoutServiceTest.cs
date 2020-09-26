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
                    var checkoutItem = new CheckoutItem { Product = product, Quantity = quantity, Price = product.UnitPrice * quantity };
                    _checkoutItems.Add(checkoutItem);
                });

            // Setting-up the ProductsRepository mock object to imitate getting an exapmle product
            _productsRepoMock.Setup(r => r.GetProduct(1)).Returns(new Product
            {
                Sku = 1,
                Name = "Bread",
                UnitPrice = 0.4m,
                MeasurmentUnit = MeasurmentUnits.UNIT
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
        /// with zero or negative price is added to the checkout
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
        /// is added with zero or negative quantity
        /// </summary>
        [Fact]
        public void Test_ExceptionWhenQuantityIsNegativeOrZero()
        {
            var service = Subject();
            // Mock getting a product from repository
            _productsRepoMock.Setup(r => r.GetProduct(1)).Returns(new Product { Sku = 1, UnitPrice = 5, MeasurmentUnit = MeasurmentUnits.POUND });
            
            // Check exeption is thrown when adding an item with negative quantity
            Assert.Throws<ArgumentOutOfRangeException>(() => service.AddItemToCheckout(1, -3, null));
            
            // Check exeption is thrown when adding an item with zero quantity
            Assert.Throws<ArgumentOutOfRangeException>(() => service.AddItemToCheckout(1, 0, null));
        }
    }
}
