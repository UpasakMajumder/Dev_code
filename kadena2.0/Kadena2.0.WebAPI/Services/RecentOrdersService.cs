using Kadena.WebAPI.Contracts;
using Kadena.WebAPI.Models.RecentOrders;
using System.Collections.Generic;
using AutoMapper;
using Kadena2.MicroserviceClients.Contracts;
using System.Threading.Tasks;
using Kadena.WebAPI.Models;

namespace Kadena.WebAPI.Services
{
    public class RecentOrdersService : IRecentOrderService
    {
        private readonly IMapper _mapper;
        private readonly IOrderViewClient _orderClient;
        private readonly IKenticoResourceService _kentico;

        public RecentOrdersService(IMapper mapper, IOrderViewClient orderClient, IKenticoResourceService kentico)
        {
            _mapper = mapper;
            _orderClient = orderClient;
            _kentico = kentico;
        }

        public async Task<OrderHead> GetHeaders()
        {
            var url = _kentico.GetSettingsKey("KDA_OrdersBySiteUrl");
            var siteName = _kentico.GetKenticoSite().Name;
            var quantity = int.Parse(_kentico.GetSettingsKey("KDA_RecentOrdersPageCapacity"));

            var response = await _orderClient.GetOrders(url, siteName, 1, quantity);

            var orderList = _mapper.Map<OrderList>(response.Payload);

            return new OrderHead
            {
                Headings = new List<string> {
                    "order number",
                    "order date",
                    "ordered",
                    "order status",
                    "delivery date",
                    ""
                },
                Pagination = new Pagination
                {
                    RowsCount = orderList.TotalCount,
                    RowsOnPage = quantity,
                    PagesCount = orderList.TotalCount / quantity + 1
                }
            };
        }
    }
}