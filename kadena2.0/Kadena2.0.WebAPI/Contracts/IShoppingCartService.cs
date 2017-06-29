using Kadena.WebAPI.Models.Checkout;
using Kadena.WebAPI.Models.OrderDetail;
using Kadena.WebAPI.Models.SubmitOrder;
using System.Threading.Tasks;

namespace Kadena.WebAPI.Contracts
{
    public interface IShoppingCartService
    {
        Task<CheckoutPage> GetCheckoutPage();

        Task<CheckoutPage> SelectShipipng(int id);

        Task<CheckoutPage> SelectAddress(int id);
        Task<SubmitOrderResult> SubmitOrder(SubmitOrderRequest request);

        Task<CheckoutPage> ChangeItemQuantity(int id, int quantity);
        Task<CheckoutPage> RemoveItem(int id);

        Task<CheckoutPage> OrderCurrentCart();

        Task<double> EstimateTotalTax();

        Task<bool> IsSubmittable();
			
        Task<OrderDetail> GetOrderDetail(string orderId);
    }
}
