using Kadena.Models.Checkout;

namespace Kadena.WebAPI.Factories.Checkout
{
    public interface ICheckoutPageFactory
    {
        CartEmptyInfo CreateCartEmptyInfo(CartItem[] items);
    }
}