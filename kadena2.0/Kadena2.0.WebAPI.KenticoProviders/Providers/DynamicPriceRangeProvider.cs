using CMS.DocumentEngine;
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

        public decimal GetDynamicPrice(int documentId, int quantity)
        {
            var document = DocumentHelper.GetDocument(documentId, new TreeProvider(MembershipContext.AuthenticatedUser));
            var ranges = GetDynamicPricingRanges(document);

            if (ranges != null && ranges.Count() > 0)
            {
                var matchingRange = ranges.FirstOrDefault(i => quantity >= i.MinVal && quantity <= i.MaxVal);
                if (matchingRange != null)
                {
                    return matchingRange.Price;
                }
                else
                {
                    return decimal.MinusOne;
                }
            }
            else
            {
                return (decimal)document.GetDoubleValue("SKUPrice", 0);
            }
        }

        public IEnumerable<DynamicPricingRange> GetDynamicPricingRanges(int documentId)
        {
            var document = DocumentHelper.GetDocument(documentId, new TreeProvider(MembershipContext.AuthenticatedUser));
            return GetDynamicPricingRanges(document);
        }

        private IEnumerable<DynamicPricingRange> GetDynamicPricingRanges(TreeNode document)
        {
            var rawJson = document?.GetStringValue("ProductDynamicPricing", string.Empty);
            var ranges = JsonConvert.DeserializeObject<List<DynamicPricingRange>>(rawJson ?? string.Empty);

            return ranges;
        }
    }
}
