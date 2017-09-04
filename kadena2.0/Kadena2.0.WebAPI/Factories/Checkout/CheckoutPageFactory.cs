using Kadena.Models.Checkout;
using Kadena.WebAPI.KenticoProviders.Contracts;

namespace Kadena.WebAPI.Factories.Checkout
{
    public class CheckoutPageFactory : ICheckoutPageFactory
    {
        private readonly IKenticoResourceService resources;

        public CheckoutPageFactory(IKenticoResourceService resources)
        {
            this.resources = resources;
        }

        public CartEmptyInfo CreateCartEmptyInfo(CartItem[] items)
        {
            if (items != null && items.Length > 0)
                return null;

            return new CartEmptyInfo
            {
                Text = resources.GetResourceString("Kadena.Checkout.CartIsEmpty"),
                DashboardButtonText = resources.GetResourceString("Kadena.Checkout.ButtonDashboard"),
                DashboardButtonUrl = resources.GetSettingsKey("KDA_EmptyCart_DashboardUrl"),
                ProductsButtonText = resources.GetResourceString("Kadena.Checkout.ButtonProducts"),
                ProductsButtonUrl = resources.GetSettingsKey("KDA_EmptyCart_ProductsUrl")
            };
        }
    }
}