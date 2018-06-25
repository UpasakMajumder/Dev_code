using Kadena.Models.Checkout;
using Kadena.Models.SubmitOrder;

namespace Kadena.WebAPI.KenticoProviders.Contracts
{
    public interface IOrderCartItemsProvider
    {
        OrderCartItem[] GetOrderCartItems();
    }
}
