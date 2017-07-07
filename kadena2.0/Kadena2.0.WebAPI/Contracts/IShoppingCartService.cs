using Kadena.WebAPI.Models.Checkout;
using System.Threading.Tasks;

namespace Kadena.WebAPI.Contracts
{
    public interface IShoppingCartService
    {
        Task<CheckoutPage> GetCheckoutPage();

        Task<CheckoutPage> SelectShipipng(int id);

        Task<CheckoutPage> SelectAddress(int id);

        Task<CheckoutPage> ChangeItemQuantity(int id, int quantity);
        Task<CheckoutPage> RemoveItem(int id);

        Task<bool> IsSubmittable();
    }
}
