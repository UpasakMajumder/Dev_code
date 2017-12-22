using Kadena.BusinessLogic.Contracts;
using AutoMapper;
using Kadena2.MicroserviceClients.Contracts;
using Kadena.BusinessLogic.Services;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.WebAPI.KenticoProviders.Contracts;

namespace Kadena.WebAPI.Factories
{
    public class OrderListServiceFactory : IOrderListServiceFactory
    {
        private readonly IMapper _mapper;
        private readonly IOrderViewClient _orderClient;
        private readonly IKenticoUserProvider _kenticoUser;
        private readonly IKenticoResourceService _kenticoResources;
        private readonly IKenticoSiteProvider _site;
        private readonly IKenticoOrderProvider _order;
        private readonly IKenticoPermissionsProvider _permissions;
        private readonly IShoppingCartProvider _shoppingCart;
        private readonly IKenticoDocumentProvider _documents;
        private readonly IKenticoLogger _logger;

        public OrderListServiceFactory(IMapper mapper, IOrderViewClient orderClient, IKenticoUserProvider kenticoUser,
            IKenticoResourceService kenticoResources, IKenticoSiteProvider site, IKenticoOrderProvider order, IKenticoPermissionsProvider permissions,
            IShoppingCartProvider shoppingCart, IKenticoDocumentProvider documents, IKenticoLogger logger)
        {
            // TODO null checks

            _mapper = mapper;
            _orderClient = orderClient;
            _kenticoUser = kenticoUser;
            _kenticoResources = kenticoResources;
            _site = site;
            _order = order;
            _permissions = permissions;
            _shoppingCart = shoppingCart;
            _documents = documents;
            _logger = logger;
        }

        public IOrderListService GetDashboard()
        {
            return new OrderListService(_mapper, _orderClient, _kenticoUser, _kenticoResources, _site, _order, _shoppingCart, _documents, _permissions, _logger)
            {
                PageCapacityKey = "KDA_DashboardOrdersPageCapacity"
            };
        }

        public IOrderListService GetRecentOrders()
        {
            return new OrderListService(_mapper, _orderClient, _kenticoUser, _kenticoResources, _site, _order, _shoppingCart, _documents, _permissions, _logger)
            {
                PageCapacityKey = "KDA_RecentOrdersPageCapacity",
                EnablePaging = true
            };
        }
    }
}