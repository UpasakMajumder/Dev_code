using Kadena.Models;
using System.Collections.Generic;

namespace Kadena.WebAPI.KenticoProviders.Contracts
{
    public interface ITieredPriceRangeProvider
    {
        IEnumerable<TieredPricingRange> GetTieredRanges(int documentId);
        decimal GetTieredPrice(int quantity, string rawJson);
    }
}
