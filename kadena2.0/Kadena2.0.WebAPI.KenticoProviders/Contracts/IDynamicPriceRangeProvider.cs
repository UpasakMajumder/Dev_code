using CMS.Ecommerce;
using Kadena.Models;
using System.Collections.Generic;

namespace Kadena.WebAPI.KenticoProviders.Contracts
{
    public interface IDynamicPriceRangeProvider
    {
        decimal GetDynamicPrice(int quantity, IEnumerable<DynamicPricingRange> ranges);
        IEnumerable<DynamicPricingRange> GetDynamicPricingRanges(int documentId);
        IEnumerable<DynamicPricingRange> GetDynamicPricingRanges(SKUTreeNode document);
    }
}
