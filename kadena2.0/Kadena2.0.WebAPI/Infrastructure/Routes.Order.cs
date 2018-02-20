namespace Kadena.WebAPI.Infrastructure
{
    public partial class Routes
    {
        public class Order
        {
            public const string Export = "api/order/export/{format}";
            public const string Get = "api/order";
            public const string Detail = "api/order/{orderId}";
            public const string DetailLegacy = "api/orderdetail/{orderId}";
        }
    }
}