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
                Text = resources.GetResourceString("xxxx"),
                DashboardButtonText = resources.GetResourceString("xxxx"),
                DashboardButtonUrl = resources.GetSettingsKey("yyyy"),
                ProductsButtonText = resources.GetResourceString("xxxx"),
                ProductsButtonUrl = resources.GetSettingsKey("yyyy")
            };
        }
    }
}