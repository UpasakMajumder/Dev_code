using AutoMapper;
using DryIoc;
using Kadena.BusinessLogic.Contracts;
using Kadena.BusinessLogic.Factories.Checkout;
using Kadena.BusinessLogic.Services;
using Kadena.BusinessLogic.Services.SettingsSynchronization;
using Kadena.Helpers;
using Kadena.WebAPI.Factories;
using Kadena.WebAPI.Infrastructure;
using Kadena.WebAPI.KenticoProviders;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.MicroserviceClients.Clients;
using Kadena2.MicroserviceClients.Contracts;
using Kadena2.MicroserviceClients.Contracts.Base;
using Kadena2.WebAPI.KenticoProviders.Contracts;
using Kadena2.WebAPI.KenticoProviders.Contracts.KadenaSettings;
using Kadena2.WebAPI.KenticoProviders.Providers;
using Kadena2.WebAPI.KenticoProviders.Providers.KadenaSettings;

namespace Kadena.WebAPI
{
    public static class ContainerBuilder
    {
        public static Container RegisterBLL(this Container container)
        {
            container.Register<IShoppingCartService, ShoppingCartService>();
            container.Register<ISearchService, SearchService>();
            container.Register<ICustomerDataService, CustomerDataService>();
            container.Register<ISettingsService, SettingsService>();
            container.Register<ISiteDataService, SiteDataService>();
            container.Register<ITaxEstimationService, TaxEstimationService>();
            container.Register<IOrderService, OrderService>();
            container.Register<IKListService, KListService>();
            container.Register<ITemplateService, TemplateService>();
            container.Register<IMailTemplateService, MailTemplateService>();
            container.Register<IFavoritesService, FavoritesService>();
            container.Register<IProductsService, ProductsService>();
            container.Register<ICreditCardService, CreditCardService>();
            container.Register<IPdfService, PdfService>();
            container.Register<IBusinessUnitsService, BusinessUnitService>();
            container.Register<ICampaignsService, CampaignsService>();
            container.Register<IPOSService, POSService>();
            container.Register<IProductCategoryService, ProductCategoryService>();
            container.Register<IAddressBookService, AddressBookService>();
            container.Register<IBrandsService, BrandsService>();
            container.Register<IProgramsService, ProgramsService>();
		    container.Register<ILoginService, LoginService>();
            container.Register<ISettingsSynchronizationService, SettingsSynchronizationService>();
            return container;
        }

        public static Container RegisterKentico(this Container container)
        {
            container.Register<IKenticoUserProvider, KenticoUserProvider>();
            container.Register<IKenticoResourceService, KenticoResourceService>();
            container.Register<IKenticoSearchService, KenticoSearchService>();
            container.Register<IKenticoLogger, KenticoLogger>();
            container.Register<IKenticoMailProvider, KenticoMailProvider>();
            container.Register<IKenticoFavoritesProvider, KenticoFavoritesProvider>();
            container.Register<IKenticoProductsProvider, KenticoProductsProvider>();
            container.Register<ISubmissionIdProvider, SubmissionIdProvider>();
            container.Register<IKenticoDocumentProvider, KenticoDocumentProvider>();
            container.Register<IKenticoBusinessUnitsProvider, KenticoBusinessUnitsProvider>();
            container.Register<IKenticoCampaignsProvider, KenticoCampaignsProvider>();
            container.Register<IKenticoPOSProvider, KenticoPOSProvider>();
            container.Register<IKenticoProductCategoryProvider, KenticoProductCategoryProvider>();
            container.Register<IKenticoAddressBookProvider, KenticoAddressBookProvider>();
            container.Register<IKenticoBrandsProvider, KenticoBrandsProvider>();
            container.Register<IKenticoProgramsProvider, KenticoProgramsProvider>();
            container.Register<IShoppingCartProvider, ShoppingCartProvider>();
            container.Register<IKenticoLocalizationProvider, KenticoLocalizationProvider>();
            container.Register<IKenticoLoginProvider, KenticoLoginProvider>();
            container.Register<IKenticoSiteProvider, KenticoSiteProvider>();
            container.Register<IKenticoPermissionsProvider, KenticoPermissionsProvider>();
            container.Register<IKenticoOrderProvider, KenticoOrderProvider>();
            return container;
        }

        public static Container RegisterKadenaSettings(this Container container)
        {
            container.Register<IKadenaSettings, KadenaSettings>();
            container.Register<IShippingEstimationSettings, ShippingEstimationSettings>();
            return container;
        }

        public static Container RegisterMicroservices(this Container container)
        {
            container.Register<IMailingListClient, MailingListClient>();
            container.Register<IOrderSubmitClient, OrderSubmitClient>();
            container.Register<IOrderViewClient, OrderViewClient>();
            container.Register<ITaxEstimationServiceClient, TaxEstimationServiceClient>();
            container.Register<ITemplatedClient, TemplatedClient>();
            container.Register<IAddressValidationClient, AddressValidationClient>();
            container.Register<ISuppliantDomainClient, SuppliantDomain>();
            container.Register<IFileClient, FileClient>();
            container.Register<IMicroProperties, MicroProperties>();
            
            return container;
        }

        public static Container RegisterFactories(this Container container)
        {
            container.Register<IOrderListServiceFactory, OrderListServiceFactory>();
            container.Register<ICheckoutPageFactory, CheckoutPageFactory>();
            return container;
        }

        public static Container RegisterInfrastructure(this Container container)
        {
            container.RegisterInstance(typeof(IMapper), Mapper.Instance);
            container.Register<ICache>(Reuse.Singleton, Made.Of(() => new InMemoryCache()));
            return container;
        }
    }
}