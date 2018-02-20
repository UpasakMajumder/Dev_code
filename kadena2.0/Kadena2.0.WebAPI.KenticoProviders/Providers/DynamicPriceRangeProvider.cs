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
    public class DynamicPriceRangeProvider : IDynamicPriceRangeProvider
    {
        private readonly IKenticoResourceService resources;

        public DynamicPriceRangeProvider(IKenticoResourceService resources)
        {
            if (resources == null)
            {
                throw new ArgumentNullException(nameof(resources));
            }
            this.resources = resources;
        }


        public decimal GetDynamicPrice(int quantity, int documentId)
        {
            var document = DocumentHelper.GetDocument(documentId, new TreeProvider(MembershipContext.AuthenticatedUser)) as SKUTreeNode;
            var ranges = GetDynamicPricingRanges(document?.GetStringValue("ProductDynamicPricing", string.Empty));
            return GetDynamicPrice(quantity, ranges);
        }

        public decimal GetDynamicPrice(int quantity, string rangesJson)
        {
            var ranges = GetDynamicPricingRanges(rangesJson);
            return GetDynamicPrice(quantity, ranges);
        }

        private decimal GetDynamicPrice(int quantity, IEnumerable<DynamicPricingRange> ranges)
        {
            if (ranges != null && ranges.Count() > 0)
            {
                var matchingRange = ranges.FirstOrDefault(i => quantity >= i.MinVal && quantity <= i.MaxVal);
                if (matchingRange != null)
                {
                    return matchingRange.Price;
                }
                else
                {
                    throw new ArgumentException(resources.GetResourceString("Kadena.Product.QuantityOutOfRange"));
                }
            }
            return decimal.MinusOne;
        }

        private IEnumerable<DynamicPricingRange> GetDynamicPricingRanges(string rawJson)
        {
            var ranges = JsonConvert.DeserializeObject<List<DynamicPricingRange>>(rawJson ?? string.Empty);
            return ranges;
        }
    }
}
