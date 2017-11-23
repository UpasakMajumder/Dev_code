using Kadena.WebAPI.Contracts;
using Kadena2.MicroserviceClients.Contracts;
using System.Threading.Tasks;

namespace Kadena.WebAPI.Services
{
    public class PdfService : IPdfService
    {
        private readonly IOrderViewClient orderViewClient;

        public PdfService(IOrderViewClient orderViewClient)
        {
            this.orderViewClient = orderViewClient;
        }

        public async Task<string> GetHiresPdfLink(string orderId, int line)
        {
            var order = await orderViewClient.GetOrderByOrderId(orderId);
            return await Task.FromResult("http://www.pes.cz");
        }
    }
}