using Kadena.Models.AddToCart;
using Kadena.Models.CustomerData;

namespace Kadena.BusinessLogic.Contracts
{
    public interface IDistributorShoppingCartService
    {
        DistributorCart GetCartDistributorData(int skuID, int inventoryType = 1);
        int UpdateDistributorCarts(DistributorCart cartDistributorData);
        string UpdateCartQuantity(Distributor submitRequest);
    }
}
