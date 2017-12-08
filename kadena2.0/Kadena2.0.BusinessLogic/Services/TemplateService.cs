using System;
using Kadena.BusinessLogic.Contracts;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.MicroserviceClients.Contracts;
using System.Threading.Tasks;
using Kadena.Models.TemplatedProduct;
using System.Linq;
using System.Text;
using Kadena.Models.Product;
using Kadena.Helpers;
using System.Web;

namespace Kadena.BusinessLogic.Services
{
    public class TemplateService : ITemplateService
    {
        private readonly IKenticoResourceService _resources;
        private readonly IKenticoLogger _logger;
        private readonly ITemplatedClient _templateClient;
        private readonly IKenticoProviderService _kentico;
        private readonly IKenticoUserProvider _users;
        private readonly IKenticoDocumentProvider documents;

        public TemplateService(IKenticoResourceService resources, IKenticoLogger logger, ITemplatedClient templateClient, IKenticoProviderService kentico, IKenticoUserProvider users, IKenticoDocumentProvider documents)
        {
            this._resources = resources;
            this._logger = logger;
            this._templateClient = templateClient;
            this._kentico = kentico;
            this._users = users;
            this.documents = documents;
        }

        public async Task<bool> UpdateTemplate(Guid templateId, string name, int quantity)
        {
            var result = await _templateClient.UpdateTemplate(templateId, name, quantity);
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
                OpenInDesignBtn = _resources.GetResourceString("Kadena.Product.ManageProducts.OpenInDesign"),
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

            var requestResult = await _templateClient
                .GetTemplates(_users.GetCurrentUser().UserId, product.ProductChiliTemplateID);

            var productEditorUrl = _resources.GetSettingsKey("KDA_Templating_ProductEditorUrl")?.TrimStart('~');
            if (string.IsNullOrWhiteSpace(productEditorUrl))
            {
                _logger.LogError("GET TEMPLATE LIST", "Product editor URL is not configured");
            }
            else
            {
                productEditorUrl = documents.GetDocumentUrl(productEditorUrl);
            }

            if (requestResult.Success)
            {
                var defaultQuantity = 1;
                productTemplates.Data = requestResult.Payload
                    .Select(d =>
                    {
                        int quantity = defaultQuantity;
                        if (d.MailingList != null)
                        {
                            quantity = d.MailingList.RowCount;
                        }
                        else
                        {
                            if (d.MetaData.ContainsKey(nameof(quantity)))
                            {
                                quantity = int.Parse(d.MetaData[nameof(quantity)].ToString());
                            }
                        }

                        return new ProductTemplate
                        {
                            EditorUrl = BuildTemplateEditorUrl(productEditorUrl, nodeId, d.TemplateId.ToString(),
                                product.ProductChiliWorkgroupID.ToString(), quantity, d.MailingList?.ContainerId, d.Name),
                            TemplateId = d.TemplateId,
                            CreatedDate = DateTime.Parse(d.Created),
                            UpdatedDate = DateTime.Parse(d.Updated),
                            ProductName = d.Name
                        };
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