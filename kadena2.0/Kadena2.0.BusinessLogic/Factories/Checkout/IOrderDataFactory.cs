using Kadena.Dto.SubmitOrder.MicroserviceRequests;
using Kadena.Models;
using Kadena.Models.SubmitOrder;

namespace Kadena.BusinessLogic.Factories.Checkout
{
    public interface IOrderDataFactory
    {
        AddressDTO CreateBillingAddress(BillingAddress billingAddress);
        AddressDTO CreateShippingAddress(DeliveryAddress shippingAddress, Customer customer);
        CustomerDTO CreateCustomer(Customer customer);
        PaymentOptionDTO CreatePaymentOption(Models.PaymentMethod paymentMethod, SubmitOrderRequest request);
    }
}