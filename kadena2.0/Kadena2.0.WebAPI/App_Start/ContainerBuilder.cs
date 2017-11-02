using Amazon.SecurityToken;
using AutoMapper;
using DryIoc;
using Kadena.KOrder.PaymentService.Infrastucture.Helpers;
using Kadena.WebAPI.Contracts;
using Kadena.WebAPI.Factories;
using Kadena.WebAPI.Factories.Checkout;
using Kadena.WebAPI.Infrastructure;
using Kadena.WebAPI.KenticoProviders;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena.WebAPI.Services;
using Kadena2.MicroserviceClients.Clients;
using Kadena2.MicroserviceClients.Contracts;

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
            return container;
        }

        public static Container RegisterKentico(this Container container)
        {
            container.Register<IKenticoProviderService, KenticoProviderService>();
            container.Register<IKenticoUserProvider, KenticoUserProvider>();
            container.Register<IKenticoResourceService, KenticoResourceService>();
            container.Register<IKenticoSearchService, KenticoSearchService>();
            container.Register<IKenticoLogger, KenticoLogger>();
            container.Register<IKenticoMailProvider, KenticoMailProvider>();			
            container.Register<IKenticoFavoritesProvider, KenticoFavoritesProvider>();
            container.Register<IKenticoProductsProvider, KenticoProductsProvider>();
            return container;
        }

        public static Container RegisterMicroservices(this Container container)
        {
            //container.Register<IAwsV4Signer, DefaultAwsV4Signer>();
            container.Register<IMailingListClient, MailingListClient>();
            container.Register<IOrderSubmitClient, OrderSubmitClient>();
            container.Register<IOrderViewClient, OrderViewClient>();
            container.Register<ITaxEstimationServiceClient, TaxEstimationServiceClient>();
            container.Register<ITemplatedClient, TemplatedClient>();
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
            container.Register<IBackgroundTaskScheduler, BackgroundTaskScheduler>();
            container.Register<ICache>(Reuse.Singleton, Made.Of(() => new InMemoryCache()));            
            //container.Register<IAmazonSecurityTokenService, AmazonSecurityTokenServiceClient>();
            return container;
        }
    }
}