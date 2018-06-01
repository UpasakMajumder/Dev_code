using CMS.DocumentEngine;
using CMS.Ecommerce;
using CMS.Membership;
using Kadena.Models;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kadena.WebAPI.KenticoProviders.Providers
{
    public class TieredPriceRangeProvider : ITieredPriceRangeProvider
    {
        private readonly IKenticoResourceService resources;

        public TieredPriceRangeProvider(IKenticoResourceService resources)
        {
            this.resources = resources ?? throw new ArgumentNullException(nameof(resources));
        }

        public IEnumerable<TieredPricingRange> GetTieredRanges(int documentId)
        {
            var document = DocumentHelper.GetDocument(documentId, new TreeProvider(MembershipContext.AuthenticatedUser)) as SKUTreeNode;
            var rawJson = document?.GetStringValue("ProductTieredPricing", string.Empty);
            return ParseJson(rawJson);
        }

        private IEnumerable<TieredPricingRange> ParseJson(string rawJson)
        {
            return JsonConvert.DeserializeObject<List<TieredPricingRange>>(rawJson);
        }

        public decimal GetTieredPrice(int quantity, string rawJson)
        {
            var ranges = ParseJson(rawJson);
            return GetTieredPrice(quantity, ranges);
        }

        private decimal GetTieredPrice(int quantity, IEnumerable<TieredPricingRange> ranges)
        {
            if (ranges != null && ranges.Count() > 0)
            {
                var matchingRange = ranges.FirstOrDefault(i => i.Quantity == quantity);
                if (matchingRange != null)
                {
                    return matchingRange.Price / (decimal)matchingRange.Quantity;
                }
                else
                {
                    throw new ArgumentException(resources.GetResourceString("Kadena.Product.QuantityOutOfRange"));
                }
            }
            return decimal.MinusOne;
        }
    }
}
