using Microsoft.AspNetCore.Mvc;
using SupermarketPricingKata.Services;
using SupermarketPricingKata.ViewModels;

namespace SupermarketPricingKata.Controllers
{
    /// <summary>
    /// Checkout API MVC controller class.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CheckoutController : ControllerBase
    {
        private readonly ICheckoutService _checkoutService;
        private readonly IProductsService _productsService;

        /// <summary>
        /// Controller constructor used to inject services by dependency injection.
        /// </summary>
        /// <param name="checkoutService">An instance of <see cref="ICheckoutService"/>.</param>
        /// <param name="productsService">An instance of <see cref="IProductsService"/></param>
        public CheckoutController(ICheckoutService checkoutService, IProductsService productsService)
        {
            _checkoutService = checkoutService;
            _productsService = productsService;
        }

        /// <summary>
        /// Performs a HTTP GET request to get the list of available products.
        /// </summary>
        /// <returns>
        /// A HTTP status code 200 OK and the products list
        /// if the operation is successful; otherwise 404 NOT FOUND.
        /// </returns>
        [HttpGet("products")]
        public IActionResult GetProducts()
        {
            var products = _productsService.GetAllProducts();
            return Ok(products);
        }

        /// <summary>
        /// Performs a HTTP GET request to get a product based on its <paramref name="sku"/>.
        /// </summary>
        /// <param name="sku">Route parameter representing the product identifier in the data store.</param>
        /// <returns>
        /// A HTTP status code 200 OK and the searched products 
        /// if the operation is successful; otherwise 404 NOT FOUND.
        /// </returns>
        [HttpGet("products/{sku}")]
        public IActionResult GetProduct(int sku)
        {
            var product = _productsService.GetProduct(sku);
            return Ok(product);
        }

        /// <summary>
        /// Performs a HTTP GET request to get the <see cref="ViewModels.Checkout"/> object.
        /// </summary>
        /// <returns>
        /// A HTTP status code 200 OK and the checkout object 
        /// if the operation is successful; otherwise 404 NOT FOUND.
        /// </returns>
        [HttpGet("checkout")]
        public IActionResult GetCheckout()
        {
            var checkoutItems = _checkoutService.GetCheckoutItems();
            // Instanciate a checkout object with the available checkout items and calculate the checkout total price
            var checkout = new Checkout { CheckoutItems = checkoutItems, TotalPrice = _checkoutService.CalculateTotal() };
            return Ok(checkout);
        }

        /// <summary>
        /// Performs a HTTP GET request to get the list of available discount rules.
        /// </summary>
        /// <returns>
        /// A HTTP status code 200 OK and the discount rules list
        /// if the operation is successful; otherwise 404 NOT FOUND.
        /// </returns>
        [HttpGet("discounts")]
        public IActionResult GetDiscountRules()
        {
            var discounts = _checkoutService.GetDiscountRules();
            return Ok(discounts);
        }

        /// <summary>
        /// Performs a HTTP POST request to add an item to the checkout list.
        /// </summary>
        /// <param name="item">HTTP POST request body containing the item to be added.</param>
        /// <returns>HTTP status code 201 CREATED and the item that has been added.</returns>
        [HttpPost("addToCart")]
        public IActionResult AddItemToCheckout([FromBody] DiscountedCheckoutItem item)
        {
            _checkoutService.AddItemToCheckout(item.Product.Sku, item.Quantity, item.DiscountRule);
            return CreatedAtAction(
                "GetCheckoutItem",
                new { id = item.Product.Sku },
                item
            );
        }

        /// <summary>
        /// Performs a HTTP DELETE request to remove an item from the checkout
        /// based on its <paramref name="id"/>.
        /// </summary>
        /// <param name="id">Route parameter representing the checkout item's identifier</param>
        /// <returns>A HTTP status code 200 OK</returns>
        [HttpDelete("deleteItem/{id}")]
        public IActionResult DeleteItemFromCheckout(int id)
        {
            _checkoutService.DeleteItemFromCheckout(id);
            return Ok();
        }
    }
}
