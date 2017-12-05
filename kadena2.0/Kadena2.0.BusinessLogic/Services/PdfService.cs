using Kadena.BusinessLogic.Contracts;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.MicroserviceClients.Contracts;
using System;
using System.Threading.Tasks;

namespace Kadena.BusinessLogic.Services
{
    public class PdfService : IPdfService
    {
        private readonly IOrderViewClient orderViewClient;
        private readonly IFileClient fileClient;
        private readonly IKenticoResourceService resources;
        private readonly IKenticoDocumentProvider documents;
        private readonly IKenticoLogger logger;

        public PdfService(IOrderViewClient orderViewClient, IFileClient fileClient, IKenticoResourceService resources, IKenticoDocumentProvider documents, IKenticoLogger logger)
        {
            if (orderViewClient == null)
            {
                throw new ArgumentNullException(nameof(orderViewClient));
            }
            if (fileClient == null)
            {
                throw new ArgumentNullException(nameof(fileClient));
            }
            if (resources == null)
            {
                throw new ArgumentNullException(nameof(resources));
            }
            if (documents == null)
            {
                throw new ArgumentNullException(nameof(documents));
            }
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            this.orderViewClient = orderViewClient;
            this.fileClient = fileClient;
            this.resources = resources;
            this.documents = documents;
            this.logger = logger;
        }

        public async Task<string> GetHiresPdfLink(string orderId, int line)
        {
            var order = await orderViewClient.GetOrderByOrderId(orderId);

            if (!order.Success || order.Payload == null)
            {
                logger.LogError("GetHiresPdfLink", $"Failed to call OrderView microservice. {order.ErrorMessages}");
                return documents.GetDocumentAbsoluteUrl(resources.GetSettingsKey("KDA_HiresPdfLinkFail"));
            }

            var orderLines = order.Payload.Items?.Count ?? 0;

            if (orderLines == 0 || orderLines < (line - 1))
            {
                logger.LogError("GetHiresPdfLink", $"Order doesn't contain line #{line}");
                return GetCustomizedFailUrl(order.Payload.SiteId);
            }

            var fileInfo = order.Payload.Items[line].FileInfo;

            var linkResult = await fileClient.GetShortliveSecureLink(fileInfo.Key, fileInfo.Module);


            if (!linkResult.Success || string.IsNullOrEmpty(linkResult.Payload))
            {
                logger.LogError("GetHiresPdfLink", $"Failed to call File microservice. {order.ErrorMessages}");
                return GetCustomizedFailUrl(order.Payload.SiteId);
            }

            return linkResult.Payload;
        }

        private string GetCustomizedFailUrl(int siteId)
        {
            return documents.GetDocumentAbsoluteUrl(resources.GetSettingsKey(siteId, "KDA_HiresPdfLinkFail"));
        }
    }
}