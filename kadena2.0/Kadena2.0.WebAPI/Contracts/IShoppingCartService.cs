using Kadena.WebAPI.Models;
using System.Collections.Generic;

namespace Kadena.WebAPI.Contracts
{
    public interface IShoppingCartService
    {
        CheckoutPage GetCheckoutPage();
        List<PaymentMethod> OrderPaymentMethods(PaymentMethod[] methods);

        CheckoutPage SelectShipipng(int id);

        CheckoutPage SelectAddress(int id);
    }
}
