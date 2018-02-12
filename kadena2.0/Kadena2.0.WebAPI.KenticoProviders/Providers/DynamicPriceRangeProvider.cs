using CMS.DocumentEngine;
using CMS.Ecommerce;
using CMS.Membership;
using Kadena.Models;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Kadena.WebAPI.KenticoProviders.Providers
{
    public class DynamicPriceRangeProvider : IDynamicPriceRangeProvider
    {
        public decimal GetDynamicPrice(int quantity, IEnumerable<DynamicPricingRange> ranges)
        {
            if (ranges != null)
            {
                var matchingRange = ranges.FirstOrDefault(i => quantity >= i.MinVal && quantity <= i.MaxVal);
                if (matchingRange != null)
                {
                    return matchingRange.Price;
                }
            }
            return 0.0m;
        }

        public IEnumerable<DynamicPricingRange> GetDynamicPricingRanges(int documentId)
        {
            var document = DocumentHelper.GetDocument(documentId, new TreeProvider(MembershipContext.AuthenticatedUser)) as SKUTreeNode;
            return GetDynamicPricingRanges(document);
        }

        public IEnumerable<DynamicPricingRange> GetDynamicPricingRanges(SKUTreeNode document)
        {
            var rawJson = document?.GetStringValue("ProductDynamicPricing", string.Empty);
            var ranges = JsonConvert.DeserializeObject<List<DynamicPricingRange>>(rawJson ?? string.Empty);
            return ranges;
        }
    }
}
