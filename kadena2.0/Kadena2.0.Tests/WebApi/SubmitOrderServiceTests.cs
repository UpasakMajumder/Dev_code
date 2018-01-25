using Kadena.BusinessLogic.Services.Orders;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2._0.BusinessLogic.Contracts.Orders;
using Kadena2.BusinessLogic.Contracts.OrderPayment;
using Moq;

namespace Kadena.Tests.WebApi
{
    public class SubmitOrderServiceTests
    {
        private SubmitOrderService CreateSubmitOrderService()
        {
            var shoppingCart = new Mock<IShoppingCartProvider>();
            var card3dsi = new Mock<ICreditCard3dsi>();
            var purchaseOrder = new Mock<IPurchaseOrder>();
            var orderData = new Mock<IGetOrderDataService>();

            return new SubmitOrderService(shoppingCart.Object,
                orderData.Object,
                card3dsi.Object,
                purchaseOrder.Object
            );
        }
    }
}
