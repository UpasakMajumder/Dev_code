using Kadena.BusinessLogic.Contracts;
using Kadena2.MicroserviceClients.Contracts;
using System;
using System.Threading.Tasks;

namespace Kadena.BusinessLogic.Services
{
    public class PdfService : IPdfService
    {
        private readonly IOrderViewClient orderViewClient;
        private readonly IFileClient fileClient;

        public PdfService(IOrderViewClient orderViewClient, IFileClient fileClient)
        {
            if (orderViewClient == null)
            {
                throw new ArgumentNullException(nameof(orderViewClient));
            }
            if (fileClient == null)
            {
                throw new ArgumentNullException(nameof(fileClient));
            }

            this.orderViewClient = orderViewClient;
            this.fileClient = fileClient;
        }

        public async Task<string> GetHiresPdfLink(string orderId, int line)
        {
            var order = await orderViewClient.GetOrderByOrderId(orderId);

            if (!order.Success)
            {
                return string.Empty;
            }
            
            var orderLines = order.Payload?.Items?.Count ?? 0;

            if (orderLines == 0 || orderLines < (line-1))
            {
                return string.Empty;
            }

            var fileInfo = order.Payload.Items[line].FileInfo;

            var linkResult = await fileClient.GetShortliveSecureLink(fileInfo.Key, fileInfo.Module);

            return linkResult.Success ? linkResult.Payload : string.Empty;

            // todo properly log if something fails
            // todo redirect to some nice page in that case
        }
    }
}