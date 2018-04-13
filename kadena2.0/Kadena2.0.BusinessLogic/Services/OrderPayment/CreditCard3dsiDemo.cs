using Kadena.Models.SiteSettings;
using Kadena.Models.SubmitOrder;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.BusinessLogic.Contracts.OrderPayment;
using System;

namespace Kadena2.BusinessLogic.Services.OrderPayment
{
    public class CreditCard3dsiDemo : ICreditCard3dsiDemo
    {
        private readonly IKenticoResourceService resources;
        private readonly IKenticoDocumentProvider documents;

        public CreditCard3dsiDemo(IKenticoResourceService resources, IKenticoDocumentProvider documents)
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

        public SubmitOrderResult PayByCard3dsi()
        {
            var insertCardUrl = resources.GetSiteSettingsKey(Settings.KDA_CreditCard_InsertCardDetailsURL);

            return new SubmitOrderResult
            {
                Success = true,
                RedirectURL = documents.GetDocumentUrl(insertCardUrl)
            };
        }
    }
}
