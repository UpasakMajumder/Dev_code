using CMS.Ecommerce;
using Kadena.Models;
using System.Collections.Generic;

namespace Kadena.WebAPI.KenticoProviders.Contracts
{
    public interface IDynamicPriceRangeProvider
    {
        decimal GetDynamicPrice(int quantity, int documentId);
        decimal GetDynamicPrice(int quantity, string rangesJson);
        IEnumerable<DynamicPricingRange> GetDynamicRanges(int documentId);
    }
}
