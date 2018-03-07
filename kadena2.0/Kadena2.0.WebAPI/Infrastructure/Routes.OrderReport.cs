namespace Kadena.WebAPI.Infrastructure
{
    public partial class Routes
	{
		public class OrderReport
        {
            public const string Export = "api/order/report/export/{format}";
            public const string Get = "api/order/report";
        }
	}
}