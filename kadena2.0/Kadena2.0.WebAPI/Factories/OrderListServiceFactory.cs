using Kadena.BusinessLogic.Contracts;
using AutoMapper;
using Kadena2.MicroserviceClients.Contracts;
using Kadena.BusinessLogic.Services;
using Kadena.WebAPI.KenticoProviders.Contracts;

namespace Kadena.WebAPI.Factories
{
    public class OrderListServiceFactory : IOrderListServiceFactory
    {
        private readonly IMapper _mapper;
        private readonly IOrderViewClient _orderClient;
        private readonly IKenticoUserProvider _kenticoUser;
        private readonly IKenticoResourceService _kenticoResources;
        private readonly IKenticoProviderService _kentico;
        private readonly IKenticoDocumentProvider _documents;
        private readonly IKenticoLogger _logger;

        public OrderListServiceFactory(IMapper mapper, IOrderViewClient orderClient, IKenticoUserProvider kenticoUser,
            IKenticoResourceService kenticoResources, IKenticoProviderService kentico, IKenticoDocumentProvider documents,
            IKenticoLogger logger)
        {
            _mapper = mapper;
            _orderClient = orderClient;
            _kenticoUser = kenticoUser;
            _kenticoResources = kenticoResources;
            _kentico = kentico;
            _documents = documents;
            _logger = logger;
        }

        public IOrderListService GetDashboard()
        {
            return new OrderListService(_mapper, _orderClient, _kenticoUser, _kenticoResources, _kentico, _documents, _logger)
            {
                PageCapacityKey = "KDA_DashboardOrdersPageCapacity"
            };
        }

        public IOrderListService GetRecentOrders()
        {
            return new OrderListService(_mapper, _orderClient, _kenticoUser, _kenticoResources, _kentico, _documents, _logger)
            {
                PageCapacityKey = "KDA_RecentOrdersPageCapacity",
                EnablePaging = true
            };
        }
    }
}