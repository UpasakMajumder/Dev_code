namespace Kadena.Helpers.Routes
{
    public partial class Routes
    {
        public class Order
        {
            public const string Detail = "api/orderdetail/{orderId}";
            public const string History = "api/order/{orderId}/history";
            public const string GetToApprove = "api/orders/toApprove";
            public const string OrderUpdate = "api/orderupdate";
            public const string OrderShippingUpdate = "api/ordersshippingsupdate";
            public const string Approve = "api/order/approve";
            public const string Reject = "api/order/reject";
            public const string GetCampaignOrdersToApprove = "api/orders/toapprove/{orderType}";
        }
    }
}
