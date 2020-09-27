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
            // Mock getting a product from repository
            _productsRepoMock.Setup(r => r.GetProduct(1)).Returns(new Product { Sku = 1, UnitPrice = 5, MeasurmentUnit = MeasurmentUnits.POUND });
            
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
            // Setup mock repository to return an item with measurment unit UNIT (item is sold by number)
            _productsRepoMock.Setup(r => r.GetProduct(1)).Returns(new Product { Sku = 1, UnitPrice = 5, MeasurmentUnit = MeasurmentUnits.UNIT });
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

            _productsRepoMock.Setup(r => r.GetProduct(4)).Returns(new Product
            {
                Sku = 4,
                Name = "Milk",
                MeasurmentUnit = MeasurmentUnits.LITRE,
                UnitPrice = 1.25m
            });
            service.AddItemToCheckout(4, 3, null); // 3L of Milk

            Assert.Equal(5.54m, service.CalculateTotal());
        }
    }
}
