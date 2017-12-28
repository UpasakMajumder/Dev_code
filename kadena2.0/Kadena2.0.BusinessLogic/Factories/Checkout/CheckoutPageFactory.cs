using Kadena.Models.Checkout;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System;

namespace Kadena.BusinessLogic.Factories.Checkout
{
    public class CheckoutPageFactory : ICheckoutPageFactory
    {
        private readonly IKenticoResourceService resources;
        private readonly IKenticoDocumentProvider documents;

        public CheckoutPageFactory(IKenticoResourceService resources, IKenticoDocumentProvider documents)
        {
            if(resources == null)
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

        public CartEmptyInfo CreateCartEmptyInfo(CartItem[] items)
        {
            if (items != null && items.Length > 0)
                return null;

            return new CartEmptyInfo
            {
                Text = resources.GetResourceString("Kadena.Checkout.CartIsEmpty"),
                DashboardButtonText = resources.GetResourceString("Kadena.Checkout.ButtonDashboard"),
                DashboardButtonUrl = documents.GetDocumentUrl(resources.GetSettingsKey("KDA_EmptyCart_DashboardUrl")),
                ProductsButtonText = resources.GetResourceString("Kadena.Checkout.ButtonProducts"),
                ProductsButtonUrl = documents.GetDocumentUrl(resources.GetSettingsKey("KDA_EmptyCart_ProductsUrl"))
            };
        }
    }
}