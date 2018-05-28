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
            return JsonConvert.DeserializeObject<List<TieredPricingRange>>(rawJson);
        }
    }
}
