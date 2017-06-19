using Kadena.WebAPI.Models;
using Kadena.WebAPI.Models.SubmitOrder;
using System.Collections.Generic;
using System.Threading.Tasks;
using PaymentMethod = Kadena.WebAPI.Models.PaymentMethod;

namespace Kadena.WebAPI.Contracts
{
    public interface IShoppingCartService
    {
        Task<CheckoutPage> GetCheckoutPage();
        List<PaymentMethod> ArrangePaymentMethods(PaymentMethod[] methods);

        Task<CheckoutPage> SelectShipipng(int id);

        Task<CheckoutPage> SelectAddress(int id);
        Task<SubmitOrderResult> SubmitOrder(SubmitOrderRequest request);

        Task<CheckoutPage> ChangeItemQuantity(int id, int quantity);
        Task<CheckoutPage> RemoveItem(int id);

        Task<CheckoutPage> OrderCurrentCart();

        Task<double> EstimateTotalTax();
    }
}
