using SupermarketPricingKata.Models;
using System.Collections.Generic;
using System.Linq;

namespace SupermarketPricingKata.Repositories
{
    public class DiscountsRepository : IDiscountsRepository
    {
        // Predefined list of discount rules used as an example
        // This could be later replaced by a database connection
        private readonly IList<DiscountRule> _discountRules = new List<DiscountRule>
        {
            new DiscountRule
            {
                Id = 1,
                Description = "Buy Three for a Dollar",
                Quantity = 3
            },
            new DiscountRule
            {
                Id = 2,
                Description = "Buy two, get one free",
                Quantity = 3
            },
            new DiscountRule
            {
                Id = 3,
                Description = "80% Off",
                Quantity = 1
            },
            new DiscountRule
            {
                Id = 4,
                Description = "50% Off",
                Quantity = 1
            }
        };

        public DiscountRule GetDiscountRule(int id)
        {
            return _discountRules.SingleOrDefault(r => r.Id == id);
        }

        public IList<DiscountRule> GetDiscountRules()
        {
            return _discountRules.ToList();
        }
    }
}
