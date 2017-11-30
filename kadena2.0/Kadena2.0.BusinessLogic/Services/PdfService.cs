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

            // fileurl will disapead, there will be new property fileInfo : { module ,  key }

            var s3url = "http://www.pes.cz";
            //todo var s3url = call File service to ask secure shortlive link (module, key)

            // todo what to do if anything fails and no url is retrieved ? Redirect to some error page ?

            return s3url;
        }
    }
}