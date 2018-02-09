using Kadena.Models;
using System.Collections.Generic;

namespace Kadena.WebAPI.KenticoProviders.Contracts
{
    public interface IDynamicPriceRangeProvider
    {
        decimal GetDynamicPrice(int quantity, IEnumerable<DynamicPricingRange> ranges);
        decimal GetDynamicPrice(int documentId, int quantity);
        IEnumerable<DynamicPricingRange> GetDynamicPricingRanges(int documentId);
    }
}
