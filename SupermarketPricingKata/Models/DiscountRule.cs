using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SupermarketPricingKata.Models
{
    /// <summary>
    /// Models a discount rule to be applied on a product.
    /// </summary>
    public class DiscountRule
    {
        /// <summary>
        /// Discount rule identifier.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// The discout rule description.
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// The quantity of products on which the discount is applied.
        /// </summary>
        public decimal Quantity { get; set; }
        
        /// <summary>
        /// The new price corresponding to the <see cref="Quantity"/> of products with discount applied.
        /// </summary>
        public decimal Price { get; set; }
    }
}
