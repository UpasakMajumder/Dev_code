using System;
using System.Collections.Generic;
using System.Linq;
using Kadena.BusinessLogic.Contracts;
using Kadena.Models.SiteSettings;
using Kadena.WebAPI.KenticoProviders.Contracts;

namespace Kadena.BusinessLogic.Services
{
    public class NewProductNotificationService : INewProductNotificationService
    {
        private readonly IKenticoResourceService _kenticoResourceService;
        private readonly IKenticoMailProvider _kenticoMailProvider;
        private readonly IKenticoSkuProvider _kenticoSkuProvider;
        private readonly IKenticoCustomerProvider _kenticoCustomerProvider;
        private readonly IKenticoProductsProvider _kenticoProductsProvider;
        private readonly IProductsService _productsService;
        private readonly IImageService _imageService;
        private readonly IKenticoSiteProvider _kenticoSiteProvider;

        public NewProductNotificationService(IKenticoResourceService kenticoResourceService, IKenticoMailProvider kenticoMailProvider, IKenticoSkuProvider kenticoSkuProvider, IKenticoCustomerProvider kenticoCustomerProvider, IKenticoProductsProvider kenticoProductsProvider, IProductsService productsService, IImageService imageService, IKenticoSiteProvider kenticoSiteProvider)
        {
            _kenticoResourceService =
                kenticoResourceService ?? throw new ArgumentNullException(nameof(kenticoResourceService));
            _kenticoMailProvider = kenticoMailProvider ?? throw new ArgumentNullException(nameof(kenticoMailProvider));
            _kenticoSkuProvider = kenticoSkuProvider ?? throw new ArgumentNullException(nameof(kenticoSkuProvider));
            _kenticoCustomerProvider = kenticoCustomerProvider ??
                                       throw new ArgumentNullException(nameof(kenticoCustomerProvider));
            _kenticoProductsProvider = kenticoProductsProvider ??
                                       throw new ArgumentNullException(nameof(kenticoProductsProvider));
            _productsService = productsService ?? throw new ArgumentNullException(nameof(productsService));
            _imageService = imageService ?? throw new ArgumentNullException(nameof(imageService));
            _kenticoSiteProvider = kenticoSiteProvider ?? throw new ArgumentNullException(nameof(kenticoSiteProvider));
        }

        public void SendNotification(int newSkuid)
        {
            var featureEnabled =
                _kenticoResourceService.GetSiteSettingsKey<bool>(Settings.KDA_NewProductEmailNotificationEnabled);

            if (!featureEnabled)
                return;

            var sku = _kenticoSkuProvider.GetSKU(newSkuid);
            var product = _kenticoProductsProvider.GetProductLinkBySkuid(newSkuid);
            var price = _productsService.GetPrice(newSkuid);
            product.ImageUrl = _kenticoSiteProvider.GetAbsoluteUrl(_imageService.GetThumbnailLink(product.ImageUrl));

            var manufacturerId = sku.ManufacturerID;
            var customers = _kenticoCustomerProvider.GetCustomersByManufacturerID(manufacturerId);

            var templateData = new Dictionary<string, object>
            {
                { "productName", product.Title },
                { "description", sku.Description},
                { "price", price.Value },
                { "thumbnail", product.ImageUrl },
            };

            var emailTemplateCodeName =
                _kenticoResourceService.GetSiteSettingsKey(Settings.KDA_NewProductEmailNotificationTemplate);

            _kenticoMailProvider.SendKenticoEmail(customers.Select(x => x.Email).ToArray(), templateData, emailTemplateCodeName);
        }
    }
}
