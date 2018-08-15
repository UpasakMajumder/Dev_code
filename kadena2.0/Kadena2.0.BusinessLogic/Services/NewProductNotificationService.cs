using System;
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

        public NewProductNotificationService(IKenticoResourceService kenticoResourceService, IKenticoMailProvider kenticoMailProvider, IKenticoSkuProvider kenticoSkuProvider, IKenticoCustomerProvider kenticoCustomerProvider, IKenticoProductsProvider kenticoProductsProvider, IProductsService productsService, IImageService imageService)
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
            product.ImageUrl = _imageService.GetThumbnailLink(product.ImageUrl);

            var manufacturerId = sku.ManufacturerID;
            var customers = _kenticoCustomerProvider.GetCustomersByManufacturerID(manufacturerId);

            _kenticoMailProvider.SendNewProductNotification(customers, sku, product, price);
        }
    }
}
