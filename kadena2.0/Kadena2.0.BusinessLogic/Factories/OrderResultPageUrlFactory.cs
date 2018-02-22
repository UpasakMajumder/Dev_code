﻿using Kadena.WebAPI.KenticoProviders.Contracts;
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

        public string GetOrderResultPageUrl(bool orderSuccess, string orderId)
        {
            var redirectUrlBase = resources.GetSettingsKey("KDA_OrderSubmittedUrl");
            return GetResultPageUrl(redirectUrlBase, orderSuccess, orderId);
        }

        public string GetCardPaymentResultPageUrl(bool orderSuccess, string orderId, string submissionId = "", string error = "")
        {
            var redirectUrlBase = resources.GetSettingsKey("KDA_CreditCard_PaymentResultPage");
            return GetResultPageUrl(redirectUrlBase, orderSuccess, orderId, submissionId, error);
        }

        private string GetResultPageUrl(string baseUrl, bool orderSuccess, string orderId, string submissionId = "", string error = "")
        {
            var redirectUrlBaseLocalized = documents.GetDocumentUrl(baseUrl);
            var redirectUrl = $"{redirectUrlBaseLocalized}?success={orderSuccess}".ToLower();
            if (orderSuccess)
            {
                redirectUrl += "&order_id=" + orderId;
            }
            else
            {
                redirectUrl += "&submissionId=" + submissionId;
            }
            if (!string.IsNullOrEmpty(error))
            {
                redirectUrl += "&error=" + error;
            }
            return redirectUrl;
        }
    }
}
