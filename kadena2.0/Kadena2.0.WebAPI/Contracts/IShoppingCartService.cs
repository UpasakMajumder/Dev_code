using Kadena.WebAPI.Models;
using Kadena.WebAPI.Models.SubmitOrder;
using System.Collections.Generic;
using System.Threading.Tasks;
using PaymentMethod = Kadena.WebAPI.Models.PaymentMethod;

namespace Kadena.WebAPI.Contracts
{
    public interface IShoppingCartService
    {
        CheckoutPage GetCheckoutPage();
        List<PaymentMethod> OrderPaymentMethods(PaymentMethod[] methods);

        CheckoutPage SelectShipipng(int id);

        CheckoutPage SelectAddress(int id);
        Task<SubmitOrderResult> SubmitOrder(SubmitOrderRequest request);

        CheckoutPage ChangeItemQuantity(int id, int quantity);
        CheckoutPage RemoveItem(int id);
    }
}
