using Kadena.BusinessLogic.Contracts;
using AutoMapper;
using Kadena2.MicroserviceClients.Contracts;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.WebAPI.KenticoProviders.Contracts;
using Kadena.BusinessLogic.Services.Orders;
using System;
using Kadena.Models.SiteSettings;

namespace Kadena.BusinessLogic.Factories
{
    public class OrderListServiceFactory : IOrderListServiceFactory
    {
        private readonly IMapper _mapper;
        private readonly IOrderViewClient _orderClient;
        private readonly IKenticoCustomerProvider _kenticoCustomer;
        private readonly IKenticoResourceService _kenticoResources;
        private readonly IKenticoSiteProvider _site;
        private readonly IKenticoOrderProvider _order;
        private readonly IKenticoPermissionsProvider _permissions;
        private readonly IKenticoDocumentProvider _documents;
        private readonly IKenticoLogger _logger;
        private readonly IKenticoCustomerProvider _customers;
        private readonly IKenticoAddressBookProvider _kenticoAddressBook;

        public OrderListServiceFactory(IMapper mapper,
                                       IOrderViewClient orderClient,
                                       IKenticoCustomerProvider kenticoCustomer,
                                       IKenticoResourceService kenticoResources, 
                                       IKenticoSiteProvider site, 
                                       IKenticoOrderProvider order, 
                                       IKenticoPermissionsProvider permissions,
                                       IKenticoDocumentProvider documents, 
                                       IKenticoLogger logger,
                                       IKenticoCustomerProvider customers,
                                       IKenticoAddressBookProvider kenticoAddressBook)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _orderClient = orderClient ?? throw new ArgumentNullException(nameof(orderClient));
            _kenticoCustomer = kenticoCustomer ?? throw new ArgumentNullException(nameof(kenticoCustomer));
            _kenticoResources = kenticoResources ?? throw new ArgumentNullException(nameof(kenticoResources));
            _site = site ?? throw new ArgumentNullException(nameof(site));
            _order = order ?? throw new ArgumentNullException(nameof(order));
            _permissions = permissions ?? throw new ArgumentNullException(nameof(permissions));
            _documents = documents ?? throw new ArgumentNullException(nameof(documents));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _customers = customers ?? throw new ArgumentNullException(nameof(customers));
            _kenticoAddressBook = kenticoAddressBook ?? throw new ArgumentNullException(nameof(kenticoAddressBook));
        }

        public IOrderListService GetDashboard()
        {
            return new OrderListService(_mapper, _orderClient, _kenticoCustomer, _kenticoResources, _site, _order, _documents, _permissions, _logger, _customers, _kenticoAddressBook)
            {
                PageCapacityKey = Settings.KDA_DashboardOrdersPageCapacity
            };
        }

        public IOrderListService GetRecentOrders()
        {
            return new OrderListService(_mapper, _orderClient, _kenticoCustomer, _kenticoResources, _site, _order, _documents, _permissions, _logger, _customers, _kenticoAddressBook)
            {
                PageCapacityKey = Settings.KDA_RecentOrdersPageCapacity,
                EnablePaging = true
            };
        }
    }
}