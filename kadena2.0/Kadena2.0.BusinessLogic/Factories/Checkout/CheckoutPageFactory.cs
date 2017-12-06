using Kadena.Models.Checkout;
using Kadena.WebAPI.KenticoProviders.Contracts;

namespace Kadena.BusinessLogic.Factories.Checkout
{
    public class CheckoutPageFactory : ICheckoutPageFactory
    {
        private readonly IKenticoResourceService resources;
        private readonly IKenticoProviderService _provider;

        public CheckoutPageFactory(IKenticoResourceService resources, IKenticoProviderService provider)
        {
            this.resources = resources;
            _provider = provider;
        }

        public CartEmptyInfo CreateCartEmptyInfo(CartItem[] items)
        {
            if (items != null && items.Length > 0)
                return null;

            return new CartEmptyInfo
            {
                Text = resources.GetResourceString("Kadena.Checkout.CartIsEmpty"),
                DashboardButtonText = resources.GetResourceString("Kadena.Checkout.ButtonDashboard"),
                DashboardButtonUrl = _provider.GetDocumentUrl(resources.GetSettingsKey("KDA_EmptyCart_DashboardUrl")),
                ProductsButtonText = resources.GetResourceString("Kadena.Checkout.ButtonProducts"),
                ProductsButtonUrl = _provider.GetDocumentUrl(resources.GetSettingsKey("KDA_EmptyCart_ProductsUrl"))
            };
        }
    }
}