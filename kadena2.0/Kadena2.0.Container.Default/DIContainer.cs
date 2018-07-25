using AutoMapper;
using DryIoc;
using Kadena.BusinessLogic.Contracts;
using Kadena.BusinessLogic.Contracts.Orders;
using Kadena.BusinessLogic.Factories;
using Kadena.BusinessLogic.Factories.Checkout;
using Kadena.BusinessLogic.Services;
using Kadena.BusinessLogic.Services.Orders;
using Kadena.BusinessLogic.Services.SettingsSynchronization;
using Kadena.Helpers;
using Kadena.MicroserviceClients.Contracts;
using Kadena.MicroserviceClients.Clients;
using Kadena.WebAPI.KenticoProviders;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena.WebAPI.KenticoProviders.Providers;
using Kadena2.BusinessLogic.Contracts.Orders;
using Kadena2.BusinessLogic.Services.Orders;
using Kadena2.BusinessLogic.Contracts.OrderPayment;
using Kadena2.BusinessLogic.Services.OrderPayment;
using Kadena2.Infrastructure.Contracts;
using Kadena2.Infrastructure.Services;
using Kadena2.MicroserviceClients.Clients;
using Kadena2.MicroserviceClients.Contracts;
using Kadena2.MicroserviceClients.Contracts.Base;
using Kadena2.WebAPI.KenticoProviders.Contracts;
using Kadena2.WebAPI.KenticoProviders.Contracts.KadenaSettings;
using Kadena2.WebAPI.KenticoProviders.Providers;
using Kadena2.WebAPI.KenticoProviders.Providers.KadenaSettings;
using Kadena.BusinessLogic.Contracts.SSO;
using Kadena.BusinessLogic.Services.SSO;
using System.IdentityModel.Tokens;
using Kadena.BusinessLogic.Contracts.Approval;
using Kadena.BusinessLogic.Services.Approval;

namespace Kadena.Container.Default
{
    public static class DIContainer
    {
        private static readonly IContainer container;

        public static IContainer Instance => container;

        static DIContainer()
        {
            container = new DryIoc.Container()
                .RegisterBLL()
                .RegisterKentico()
                .RegisterKadenaSettings()
                .RegisterMicroservices()
                .RegisterFactories()
                .RegisterInfrastructure();
        }

        public static T Resolve<T>()
        {
            return container.Resolve<T>();
        }

        public static IContainer RegisterBLL(this IContainer container)
        {
            container.Register<IShoppingCartService, ShoppingCartService>();
            container.Register<ISearchService, SearchService>();
            container.Register<ICustomerDataService, CustomerDataService>();
            container.Register<ISettingsService, SettingsService>();
            container.Register<ISiteDataService, SiteDataService>();
            container.Register<ITaxEstimationService, TaxEstimationService>();
            container.Register<ISubmitOrderService, SubmitOrderService>();
            container.Register<IOrderDetailService, OrderDetailService>();
            container.Register<IOrderHistoryService, OrderHistoryService>();
            container.Register<IOrderReportService, OrderReportService>();
            container.Register<IOrderResubmissionService, OrderResubmissionService>();
            container.Register<IKListService, KListService>();
            container.Register<ITemplateService, TemplateService>();
            container.Register<IMailTemplateService, MailTemplateService>();
            container.Register<IFavoritesService, FavoritesService>();
            container.Register<IProductsService, ProductsService>();
            container.Register<IPdfService, PdfService>();
            container.Register<IBusinessUnitsService, BusinessUnitService>();
            container.Register<ICampaignsService, CampaignsService>();
            container.Register<IPOSService, POSService>();
            container.Register<IProductCategoryService, ProductCategoryService>();
            container.Register<IAddressBookService, AddressBookService>();
            container.Register<IModuleAccessService, ModuleAccessService>();
            container.Register<IBrandsService, BrandsService>();
            container.Register<IProgramsService, ProgramsService>();
            container.Register<ILoginService, LoginService>();
            container.Register<IFileService, FileService>();
            container.Register<IDateTimeFormatter, DateTimeFormatter>();
            container.Register<ICreditCard3dsi, CreditCard3dsi>();
            container.Register<ICreditCard3dsiDemo, CreditCard3dsiDemo>();
            container.Register<IPurchaseOrder, PurchaseOrder>();
            container.Register<IGetOrderDataService, GetOrderDataService>();
            container.Register<ISendSubmitOrder, SendSubmitOrder>();
            container.Register<ISubmissionService, SubmissionService>();
            container.Register<IShippingCostServiceClient, ShippingCostServiceClient>();
            container.Register<ISettingsSynchronizationService, SettingsSynchronizationService>();
            container.Register<IUserBudgetService, UserBudgetService>();
            container.Register<ISavedCreditCard3dsi, SavedCreditCard3dsi>();
            container.Register<IIBTFService, IBTFService>();
            container.Register<IIdentityService, IdentityService>();
            container.Register<ISaml2Service, Saml2Service>();
            container.Register<ISaml2HandlerService, Saml2HandlerService>();
            container.Register<Saml2SecurityTokenHandler, KadenaSaml2SecurityTokenHandler>();
            container.Register<ISaml2RecipientValidator, Saml2RecipientValidator>();
            container.Register<IRoleService, RoleService>();
            container.Register<IUserService, UserService>();
            container.Register<IMailService, MailService>();
            container.Register<ILocalizationService, LocalizationService>();
            container.Register<IImageService, ImageService>();
            container.Register<IApproverService, ApproverService>();
            container.Register<IApprovalService, ApprovalService>();
            container.Register<IDialogService, DialogService>();
            container.Register<IDeliveryEstimationDataService, DeliveryEstimationDataService>();
            container.Register<IOrderManualUpdateService, OrderManualUpdateService>();
            container.Register<IDistributorShoppingCartService, DistributorShoppingCartService>();
            container.Register<IOrderItemCheckerService, OrderItemCheckerService>();
            container.Register<IConvert, XlsxConvert>();
            container.Register<IUpdateInventoryDataService, UpdateInventoryDataService>();
            return container;
        }

        public static IContainer RegisterKentico(this IContainer container)
        {
            container.Register<IKenticoUserProvider, KenticoUserProvider>();
            container.Register<IKenticoResourceService, KenticoResourceService>();
            container.Register<IKenticoSearchService, KenticoSearchService>();
            container.Register<IKenticoLogger, KenticoLogger>();
            container.Register<IKenticoMailProvider, KenticoMailProvider>();
            container.Register<IKenticoFavoritesProvider, KenticoFavoritesProvider>();
            container.Register<IKenticoProductsProvider, KenticoProductsProvider>();
            container.Register<IKenticoDocumentProvider, KenticoDocumentProvider>();
            container.Register<IKenticoBusinessUnitsProvider, KenticoBusinessUnitsProvider>();
            container.Register<IKenticoCampaignsProvider, KenticoCampaignsProvider>();
            container.Register<IKenticoPOSProvider, KenticoPOSProvider>();
            container.Register<IKenticoProductCategoryProvider, KenticoProductCategoryProvider>();
            container.Register<IKenticoAddressBookProvider, KenticoAddressBookProvider>();
            container.Register<IKenticoBrandsProvider, KenticoBrandsProvider>();
            container.Register<IKenticoProgramsProvider, KenticoProgramsProvider>();
            container.Register<IShoppingCartProvider, ShoppingCartProvider>();
            container.Register<IShoppingCartItemsProvider, ShoppingCartItemsProvider>();
            container.Register<IKenticoLocalizationProvider, KenticoLocalizationProvider>();
            container.Register<IKenticoLoginProvider, KenticoLoginProvider>();
            container.Register<IKenticoSiteProvider, KenticoSiteProvider>();
            container.Register<IKenticoPermissionsProvider, KenticoPermissionsProvider>();
            container.Register<IKenticoModuleMappingProvider, KenticoModuleMappingProvider>();
            container.Register<IKenticoOrderProvider, KenticoOrderProvider>();
            container.Register<ISubmissionIdProvider, SubmissionIdProvider>();
            container.Register<IKenticoCustomerProvider, KenticoCustomerProvider>();
            container.Register<IKenticoSettingsProvider, KenticoSettingsProvider>();
            container.Register<IDynamicPriceRangeProvider, DynamicPriceRangeProvider>();
            container.Register<IKenticoUserBudgetProvider, KenticoUserBudgetProvider>();
            container.Register<IFailedOrderStatusProvider, FailedOrderStatusProvider>();
            container.Register<IKenticoIBTFProvider, KenticoIBTFProvider>();
            container.Register<IKenticoRoleProvider, KenticoRoleProvider>();
            container.Register<IKenticoUnitOfMeasureProvider, KenticoUnitOfMeasureProvider>();
            container.Register<IKenticoMediaProvider, KenticoMediaProvider>();
            container.Register<ITieredPriceRangeProvider, TieredPriceRangeProvider>();
            container.Register<IOrderCartItemsProvider, OrderCartItemsProvider>();
            container.Register<IKenticoSkuProvider, KenticoSkuProvider>();
            return container;
        }

        public static IContainer RegisterKadenaSettings(this IContainer container)
        {
            container.Register<IKadenaSettings, KadenaSettings>();
            container.Register<IShippingEstimationSettings, ShippingEstimationSettings>();
            return container;
        }

        public static IContainer RegisterMicroservices(this IContainer container)
        {
            container.Register<IMailingListClient, MailingListClient>();
            container.Register<IOrderSubmitClient, OrderSubmitClient>();
            container.Register<IOrderHistoryClient, OrderHistoryClient>();
            container.Register<IOrderViewClient, OrderViewClient>();
            container.Register<ITaxEstimationServiceClient, TaxEstimationServiceClient>();
            container.Register<ITemplatedClient, TemplatedClient>();
            container.Register<IAddressValidationClient, AddressValidationClient>();
            container.Register<ISuppliantDomainClient, SuppliantDomain>();
            container.Register<IFileClient, FileClient>();
            container.Register<IOrderResubmitClient, OrderResubmitClient>();
            container.Register<IMicroProperties, MicroProperties>();
            container.Register<IInventoryUpdateClient, InventoryUpdateClient>();
            container.Register<IBidClient, BidClient>();
            container.Register<ICreditCardManagerClient, CreditCardManagerClient>();
            container.Register<IPaymentServiceClient, PaymentServiceClient>();
            container.Register<IUserDataServiceClient, UserDataServiceClient>();
            container.Register<ICloudEventConfiguratorClient, CloudEventConfiguratorClient>();
            container.Register<IParsingClient, ParsingClient>();
            container.Register<IStatisticsClient, StatisticsClient>();
            container.Register<IExportClient, ExportClient>();
            container.Register<INotificationClient, NotificationClient>();
            container.Register<IApprovalServiceClient, ApprovalServiceClient>();
            container.Register<IOrderManualUpdateClient, OrderManualUpdateClient>();
            return container;
        }

        public static IContainer RegisterFactories(this IContainer container)
        {
            container.Register<IOrderListServiceFactory, OrderListServiceFactory>();
            container.Register<ICheckoutPageFactory, CheckoutPageFactory>();
            container.Register<IOrderDataFactory, OrderDataFactory>();
            container.Register<IOrderResultPageUrlFactory, OrderResultPageUrlFactory>();
            container.Register<IOrderReportFactory, OrderReportFactory>();
            return container;
        }

        public static IContainer RegisterInfrastructure(this IContainer container)
        {
            container.RegisterInstance(typeof(IMapper), MapperBuilder.MapperInstance);
            container.Register<ICache>(Reuse.Singleton, Made.Of(() => new InMemoryCache()));
            return container;
        }
    }

}
