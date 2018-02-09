using Kadena.WebAPI.KenticoProviders.Contracts;
using System;

namespace Kadena.BusinessLogic.Factories
{
    public class OrderResultPageUrlFactory : IOrderResultPageUrlFactory
    {
        private readonly IKenticoResourceService resources;
        private readonly IKenticoDocumentProvider documents;

        public OrderResultPageUrlFactory(IKenticoResourceService resources, IKenticoDocumentProvider documents)
        {
            if (resources == null)
            {
                throw new ArgumentNullException(nameof(resources));
            }
            if (documents == null)
            {
                throw new ArgumentNullException(nameof(documents));
            }

            this.resources = resources;
            this.documents = documents;
        }

        public string GetOrderResultPageUrl(string baseUrl, bool orderSuccess, string orderId)
        {
            var redirectUrlBase = resources.GetSettingsKey("KDA_OrderSubmittedUrl");
            var redirectUrlBaseLocalized = documents.GetDocumentUrl(redirectUrlBase);
            var redirectUrl = $"{redirectUrlBaseLocalized}?success={orderSuccess}".ToLower();
            if (orderSuccess)
            {
                redirectUrl += "&order_id=" + orderId;
            }
            return redirectUrl;
        }
    }
}
