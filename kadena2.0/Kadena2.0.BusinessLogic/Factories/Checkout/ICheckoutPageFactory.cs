using Kadena.Models.Checkout;

namespace Kadena.BusinessLogic.Factories.Checkout
{
    public interface ICheckoutPageFactory
    {
        CartEmptyInfo CreateCartEmptyInfo(CartItem[] items);
    }
}