using Kadena.BusinessLogic.Contracts;
using Kadena.Models.SiteSettings;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.MicroserviceClients.Contracts;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Kadena.BusinessLogic.Services
{
    public class PdfService : IPdfService
    {
        private readonly IOrderViewClient orderViewClient;
        private readonly IFileClient fileClient;
        private readonly ITemplateService templatedClient;
        private readonly IKenticoResourceService resources;
        private readonly IKenticoDocumentProvider documents;
        private readonly IKenticoLogger logger;

        public PdfService(IOrderViewClient orderViewClient, IFileClient fileClient, ITemplateService templatedClient, IKenticoResourceService resources, IKenticoDocumentProvider documents, IKenticoLogger logger)
        {
            this.orderViewClient = orderViewClient ?? throw new ArgumentNullException(nameof(orderViewClient));
            this.fileClient = fileClient ?? throw new ArgumentNullException(nameof(fileClient));
            this.templatedClient = templatedClient ?? throw new ArgumentNullException(nameof(templatedClient));
            this.resources = resources ?? throw new ArgumentNullException(nameof(resources));
            this.documents = documents ?? throw new ArgumentNullException(nameof(documents));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public string GetHiresPdfUrl(string orderId, int lineNumber)
        {
            var hash = GetHash(orderId, lineNumber.ToString());
            return $"/api/pdf/hires/{orderId}/{lineNumber}?hash={hash}";
        }
        public string GetLowresPdfUrl(Guid templateId, Guid settingsId)
        {
            var hash = GetHash(templateId.ToString(), settingsId.ToString());
            return $"/api/pdf/lowres/{templateId}/{settingsId}?hash={hash}";
        }

        public async Task<string> GetLowresPdfRedirectLink(Guid templateId, Guid settingsId, string hash)
        {
            var checkHash = GetHash(templateId.ToString(), settingsId.ToString());

            if (hash != checkHash)
            {
                logger.LogError("GetLowresPdfLink", $"Failed to verify request hash");
                return documents.GetDocumentAbsoluteUrl(resources.GetSiteSettingsKey(Settings.KDA_HiresPdfLinkFail));
            }

            var pdfUri = await templatedClient.GetPreviewUri(templateId, settingsId);

            if (pdfUri == null)
            {
                return GetCustomizedFailUrl(0);
            }

            return pdfUri.AbsolutePath;
        }


        public async Task<string> GetHiresPdfRedirectLink(string orderId, int line, string hash)
        {
            var checkHash = GetHash(orderId, line.ToString());

            if (hash != checkHash)
            {
                logger.LogError("GetHiresPdfLink", $"Failed to verify request hash");
                return documents.GetDocumentAbsoluteUrl(resources.GetSiteSettingsKey(Settings.KDA_HiresPdfLinkFail));
            }

            var order = await orderViewClient.GetOrderByOrderId(orderId);

            if (!order.Success || order.Payload == null)
            {
                logger.LogError("GetHiresPdfLink", $"Failed to call OrderView microservice. {order.ErrorMessages}");
                return documents.GetDocumentAbsoluteUrl(resources.GetSiteSettingsKey(Settings.KDA_HiresPdfLinkFail));
            }

            var fileKey = order.Payload.Items?.FirstOrDefault(i => i.LineNumber == line)?.FileKey;

            if (string.IsNullOrEmpty(fileKey))
            {
                logger.LogError("GetHiresPdfLink", $"Order doesn't contain line #{line} with valid FileKey");
                return GetCustomizedFailUrl(order.Payload.SiteId);
            }

            var  linkResult = await fileClient.GetShortliveSecureLink(fileKey);

            if (!linkResult.Success || string.IsNullOrEmpty(linkResult.Payload))
            {
                logger.LogError("GetHiresPdfLink", $"Failed to call File microservice. {order.ErrorMessages}");
                return GetCustomizedFailUrl(order.Payload.SiteId);
            }

            return linkResult.Payload;
        }

        private string GetCustomizedFailUrl(int siteId)
        {
            return documents.GetDocumentAbsoluteUrl(resources.GetSettingsKey<string>(Settings.KDA_HiresPdfLinkFail, siteId));
        }

        private string GetHash(string item1, string item2)
        {
            var salt = resources.GetSiteSettingsKey(Settings.KDA_HiresPdfLinkHashSalt);
            var hashBase = $"{item1}_{item2}|{salt}";
            var shaComputer = SHA256Managed.Create();
            var hashBytes = shaComputer.ComputeHash(Encoding.ASCII.GetBytes(hashBase));
            return BitConverter.ToString(hashBytes).Replace("-", string.Empty);
        }
    }
}