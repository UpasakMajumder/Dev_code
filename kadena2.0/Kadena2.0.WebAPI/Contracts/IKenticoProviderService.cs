using Kadena.WebAPI.Models;

namespace Kadena.WebAPI.Contracts
{
    public interface IKenticoProviderService
    {
        DeliveryAddress[] GetCustomerAddresses();
        DeliveryMethod[] GetShippingCarriers();
        DeliveryService[] GetShippingOptions();
        Total[] GetShoppingCartTotals();
        PaymentMethod[] GetPaymentMethods();
    }
}
