namespace Kadena.WebAPI.Infrastructure
{
    public partial class Routes
    {
        public class Order
        {
            public const string Detail = "api/orderdetail/{orderId}";
            public const string ToApprove = "api/orderdetail/toApprove";
        }
    }
}