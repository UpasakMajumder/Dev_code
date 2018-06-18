using Kadena.BusinessLogic.Contracts;
using Kadena.Dto.TemplatedProduct.MicroserviceResponses;
using Kadena.Helpers;
using Kadena.Models.Product;
using Kadena.Models.TemplatedProduct;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena.MicroserviceClients.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Kadena.Models.SiteSettings;
using Kadena.Dto.TemplatedProduct.MicroserviceRequests;

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
            this._resources = resources ?? throw new ArgumentNullException(nameof(resources));
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this._templateClient = templateClient ?? throw new ArgumentNullException(nameof(templateClient));
            this._users = users ?? throw new ArgumentNullException(nameof(users));
            this._documents = documents ?? throw new ArgumentNullException(nameof(documents));
            this._products = products ?? throw new ArgumentNullException(nameof(products));
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

        public async Task<ProductTemplates> GetTemplatesByProduct(int documentId)
        {
            var productTemplates = new ProductTemplates
            {
                Title = _resources.GetResourceString("KADENA.PRODUCT.ManageProducts"),
                OpenInDesignBtn = _resources.GetResourceString("Kadena.Product.ManageProducts.OpenInDesign"),
                Header = new[]
                {
                    new ProductTemplatesHeader
                    {
                        Name = nameof(ProductTemplate.Image).ToCamelCase(),
                        Title = _resources.GetResourceString("KADENA.PRODUCT.IMAGE"),
                        Sorting = SortingType.None
                    },
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

            var product = _products.GetProductByDocumentId(documentId);
            if (product != null && !product.HasProductTypeFlag(ProductTypes.TemplatedProduct))
            {
                return productTemplates;
            }

            var requestResult = await _templateClient
                .GetTemplates(_users.GetCurrentUser().UserId, product.ProductMasterTemplateID);

            var productEditorUrl = _resources.GetSiteSettingsKey(Settings.KDA_Templating_ProductEditorUrl)?.TrimStart('~');
            if (string.IsNullOrWhiteSpace(productEditorUrl))
            {
                _logger.LogError("GET TEMPLATE LIST", "Product editor URL is not configured");
            }
            else
            {
                productEditorUrl = _documents.GetDocumentUrl(productEditorUrl);
            }

            bool IsNewTemplate(DateTime created, DateTime updated)
            {
                var diff = updated - created;
                var isNew = diff.TotalSeconds < 10;
                return isNew;
            }

            if (requestResult.Success)
            {
                var defaultQuantity = 1;
                productTemplates.Data = requestResult.Payload
                    .Where(t => !IsNewTemplate(t.Created, t.Updated ?? t.Created))
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
                            EditorUrl = EditorUrl.Create(productEditorUrl, documentId, product.NodeId, t.TemplateId.ToString(),
                                product.ProductChiliWorkgroupID.ToString(), quantity, product.Use3d, t.MailingList?.ContainerId, t.Name),
                            TemplateId = t.TemplateId,
                            CreatedDate = t.Created,
                            UpdatedDate = t.Updated,
                            ProductName = t.Name,
                            Image = t.ThumbnailUrls?.FirstOrDefault()
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

        public async Task<Uri> GetPreviewUri(Guid templateId, Guid settingId)
        {
            var url = await _templateClient.GetPreview(templateId, settingId);
            if (!url.Success)
            {
                _logger.LogError(this.GetType().Name, url.ErrorMessages);
                return null;
            }
            return new Uri(url.Payload);
        }


        public async Task<string> TemplatedProductEditorUrl(int documentId, int nodeId, int userId, string productType,  Guid masterTemplateID, Guid workspaceId, bool use3d)
        {
            var productEditorUrl = _documents.GetDocumentUrl(_resources.GetSiteSettingsKey(Settings.KDA_Templating_ProductEditorUrl));
            var selectMailingListUrl = _documents.GetDocumentUrl(_resources.GetSiteSettingsKey(Settings.KDA_Templating_SelectListPageUrl));

            var requestBody = new NewTemplateRequestDto
            {
                User = userId.ToString(),
                TemplateId = masterTemplateID.ToString(),
                WorkSpaceId = workspaceId.ToString(),
                UseHtmlEditor = false,
                Use3d = use3d
            };

            var newTemplateUrl = await _templateClient.CreateNewTemplate(requestBody).ConfigureAwait(false);

            if (!newTemplateUrl.Success || string.IsNullOrEmpty(newTemplateUrl.Payload))
            {
                throw new Exception("Failed to create new template : " + newTemplateUrl.ErrorMessages);
            }
            
            var uri = new Uri(newTemplateUrl.Payload);
            var newTemplateID = HttpUtility.ParseQueryString(uri.Query).Get("doc");
            var destinationUrl = EditorUrl.Create(productEditorUrl, documentId, nodeId, newTemplateID, workspaceId.ToString(), use3d: use3d);

            if (ProductTypes.IsOfType(productType, ProductTypes.MailingProduct) && ProductTypes.IsOfType(productType, ProductTypes.TemplatedProduct))
            {
                var encodedUrl = HttpUtility.UrlEncode(destinationUrl);
                return $"{selectMailingListUrl}?url={encodedUrl}";
            }
            else
            {
                return destinationUrl;
            }
        }
    }
}