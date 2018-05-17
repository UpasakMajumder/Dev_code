namespace Kadena.Helpers.Routes
{
    public partial class Routes
    {
        public class Order
        {
            public const string Detail = "api/orderdetail/{orderId}";
            public const string GetToApprove = "api/orders/toApprove";
            public const string Approve = "api/order/approve";
            public const string Reject = "api/order/reject";
        }
    }
}
