using Kadena.BusinessLogic.Contracts;
using Kadena.Dto.TemplatedProduct.MicroserviceResponses;
using Kadena.Helpers;
using Kadena.Models.Product;
using Kadena.Models.TemplatedProduct;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.MicroserviceClients.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Kadena.BusinessLogic.Services
{
    public class TemplateService : ITemplateService
    {
        private readonly IKenticoResourceService _resources;
        private readonly IKenticoLogger _logger;
        private readonly ITemplatedClient _templateClient;
        private readonly IKenticoUserProvider _users;
        private readonly IKenticoDocumentProvider _documents;
        private readonly IKenticoProductsProvider _products;

        public TemplateService(IKenticoResourceService resources, IKenticoLogger logger, ITemplatedClient templateClient, 
            IKenticoUserProvider users, IKenticoDocumentProvider documents, IKenticoProductsProvider products)
        {
            if (resources == null)
            {
                throw new ArgumentNullException(nameof(resources));
            }
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }
            if (templateClient == null)
            {
                throw new ArgumentNullException(nameof(templateClient));
            }
            if (users == null)
            {
                throw new ArgumentNullException(nameof(users));
            }
            if (documents == null)
            {
                throw new ArgumentNullException(nameof(documents));
            }
            if (products == null)
            {
                throw new ArgumentNullException(nameof(products));
            }

            this._resources = resources;
            this._logger = logger;
            this._templateClient = templateClient;
            this._users = users;
            this._documents = documents;
            this._products = products;
        }

        public async Task<bool> UpdateTemplate(Guid templateId, string name, int quantity)
        {
            var meta = new Dictionary<string, object>
            {
                { nameof(TemplateMetaData.quantity), quantity }
            };
            var result = await _templateClient.UpdateTemplate(templateId, name, meta);
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

            var product = _products.GetProductByNodeId(nodeId);
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
                productEditorUrl = _documents.GetDocumentUrl(productEditorUrl);
            }

            Func<DateTime, DateTime, bool> IsNewTemplate = (created, updated) =>
            {
                var diff = updated - created;
                var isNew = diff.TotalSeconds < 10;
                return isNew;
            };

            if (requestResult.Success)
            {
                var defaultQuantity = 1;
                productTemplates.Data = requestResult.Payload
                    .Select(t =>
                    {
                        int quantity = defaultQuantity;
                        if (t.MailingList != null)
                        {
                            quantity = t.MailingList.RowCount;
                        }
                        else
                        {
                            if (t.MetaData.quantity != null)
                            {
                                quantity = t.MetaData.quantity.Value;
                            }
                        }

                        return new ProductTemplate
                        {
                            EditorUrl = BuildTemplateEditorUrl(productEditorUrl, nodeId, t.TemplateId.ToString(),
                                product.ProductChiliWorkgroupID.ToString(), quantity, t.MailingList?.ContainerId, t.Name),
                            TemplateId = t.TemplateId,
                            CreatedDate = DateTime.Parse(t.Created),
                            UpdatedDate = DateTime.Parse(t.Updated),
                            ProductName = t.Name
                        };
                    })
                    .Where(t => !IsNewTemplate(t.CreatedDate, t.UpdatedDate ?? t.CreatedDate))
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