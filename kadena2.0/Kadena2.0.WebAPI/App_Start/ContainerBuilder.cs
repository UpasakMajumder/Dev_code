using AutoMapper;
using DryIoc;
using Kadena.WebAPI.Contracts;
using Kadena.WebAPI.Factories;
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
            return container;
        }

        public static Container RegisterKentico(this Container container)
        {
            container.Register<IKenticoProviderService, KenticoProviderService>();
            container.Register<IKenticoResourceService, KenticoResourceService>();
            container.Register<IKenticoSearchService, KenticoSearchService>();
            container.Register<IKenticoLogger, KenticoLogger>();
            return container;
        }

        public static Container RegisterMicroservices(this Container container)
        {
            container.Register<IMailingListClient, MailingListClient>();
            container.Register<IOrderSubmitClient, OrderSubmitClient>();
            container.Register<IOrderViewClient, OrderViewClient>();
            container.Register<ITaxEstimationServiceClient, TaxEstimationServiceClient>();
            container.Register<ITemplatedProductService, TemplatedProductService>();
            return container;
        }

        public static Container RegisterFactories(this Container container)
        {
            container.Register<IOrderListServiceFactory, OrderListServiceFactory>();
            return container;
        }

        public static Container RegisterInfrastructure(this Container container)
        {
            container.RegisterInstance(typeof(IMapper), Mapper.Instance);
            return container;
        }
    }
}