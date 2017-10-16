using System;
using Kadena.WebAPI.Contracts;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.MicroserviceClients.Contracts;
using System.Threading.Tasks;
using Kadena.Models.TemplatedProduct;
using Kadena.Models;
using System.Linq;
using System.Text;
using Kadena.WebAPI.Helpers;
using System.Web;
using Kadena.Models.Product;

namespace Kadena.WebAPI.Services
{
    public class TemplateService : ITemplateService
    {
        private readonly IKenticoResourceService _resources;
        private readonly IKenticoLogger _logger;
        private readonly ITemplatedProductService _templateClient;
        private readonly IKenticoProviderService _kentico;
        private readonly IKenticoUserProvider _users;

        public TemplateService(IKenticoResourceService resources, IKenticoLogger logger, ITemplatedProductService templateClient, IKenticoProviderService kentico, IKenticoUserProvider users)
        {
            _resources = resources;
            _logger = logger;
            _templateClient = templateClient;
            _kentico = kentico;
            _users = users;
        }

        public async Task<bool> SetName(Guid templateId, string name)
        {
            string endpoint = _resources.GetSettingsKey("KDA_TemplatingServiceEndpoint");
            var result = await _templateClient.SetName(endpoint, templateId, name, _kentico.GetCurrentSiteDomain());
            if (!result.Success)
            {
                _logger.LogError("Template set name", result.ErrorMessages);
            }
            return result.Success;
        }

        public async Task<ProductTemplates> GetTemplatesByProduct(int nodeId)
        {
            var productTemplates = new ProductTemplates
            {
                Title = _resources.GetResourceString("KADENA.PRODUCT.ManageProducts"),
                Header = new []
                {
                    new ProductTemplatesHeader
                    {
                        Name = nameof(ProductTemplate.ProductName).ToCamelCase(),
                        Title = _resources.GetResourceString("KADENA.PRODUCT.NAME"),
                        Sorting = SortingType.None
                    },
                    new ProductTemplatesHeader
                    {
                        Name = nameof(ProductTemplate.CreatedDate).ToCamelCase(),
                        Title = _resources.GetResourceString("KADENA.PRODUCT.DATECREATED"),
                        Sorting = SortingType.None
                    },
                    new ProductTemplatesHeader
                    {
                        Name = nameof(ProductTemplate.UpdatedDate).ToCamelCase(),
                        Title = _resources.GetResourceString("KADENA.PRODUCT.DATEUPDATED"),
                        Sorting = SortingType.Desc
                    },
                },
                Data = new ProductTemplate[0]
            };

            var product = _kentico.GetProductByNodeId(nodeId);
            if (product != null && !product.HasProductTypeFlag(ProductTypes.TemplatedProduct))
            {
                return productTemplates;
            }

            var clientEndpoint = _resources.GetSettingsKey("KDA_TemplatingServiceEndpoint");
            var requestResult = await _templateClient
                .GetTemplates(clientEndpoint,
                    _users.GetCurrentUser().UserId,
                    product.ProductChiliTemplateID,
                    _kentico.GetCurrentSiteDomain());

            var productEditorUrl = _resources.GetSettingsKey("KDA_Templating_ProductEditorUrl")?.TrimStart('~');
            if (string.IsNullOrWhiteSpace(productEditorUrl))
            {
                _logger.LogError("GET TEMPLATE LIST", "Product editor URL is not configured");
            }
            else
            {
                productEditorUrl = _kentico.GetDocumentUrl(productEditorUrl);
            }

            if (requestResult.Success)
            {
                productTemplates.Data = requestResult.Payload
                    .Select(d => new ProductTemplate
                    {
                        EditorUrl = BuildTemplateEditorUrl(productEditorUrl, nodeId, d.TemplateId.ToString(), 
                            product.ProductChiliWorkgroupID.ToString(), d.MailingList?.RowCount ?? 0, d.MailingList?.ContainerId, d.Name),
                        TemplateId = d.TemplateId,
                        CreatedDate = DateTime.Parse(d.Created),
                        UpdatedDate = DateTime.Parse(d.Updated),
                        ProductName = d.Name
                    })
                    .OrderByDescending(t => t.UpdatedDate)
                    .ToArray();
            }
            else
            {
                _logger.LogError("GET TEMPLATE LIST", requestResult.ErrorMessages);
            }

            return productTemplates;
        }

        private string BuildTemplateEditorUrl(string productEditorBaseUrl, int nodeId, string templateId, string productChiliWorkgroupID, int mailingListRowCount, 
            string containerId = null, string customName = null)
        {
            var argumentFormat = "&{0}={1}";
            var url = new StringBuilder(productEditorBaseUrl + "?nodeId=" + nodeId)
                .AppendFormat(argumentFormat, "templateId", templateId)
                .AppendFormat(argumentFormat, "workspaceid", productChiliWorkgroupID)
                .AppendFormat(argumentFormat, "quantity", mailingListRowCount);
            if (containerId != null)
            {
                url.AppendFormat(argumentFormat, "containerId", containerId);
            }
            if (!string.IsNullOrWhiteSpace(customName))
            {
                url.AppendFormat(argumentFormat, "customName", HttpUtility.UrlEncode(customName));
            }
            return url.ToString();
        }
    }
}