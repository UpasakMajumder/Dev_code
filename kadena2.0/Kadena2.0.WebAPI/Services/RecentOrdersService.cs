using Kadena.WebAPI.Contracts;
using Kadena.WebAPI.Models.RecentOrders;
using System.Collections.Generic;
using AutoMapper;
using Kadena2.MicroserviceClients.Contracts;
using System.Threading.Tasks;
using Kadena.WebAPI.Models;
using System;
using System.Linq;
using Kadena.Dto.Order;

namespace Kadena.WebAPI.Services
{
    public class RecentOrdersService : IRecentOrderService
    {
        private readonly IMapper _mapper;
        private readonly IOrderViewClient _orderClient;
        private readonly IKenticoResourceService _kentico;
        private readonly int _pageCapacity;

        public RecentOrdersService(IMapper mapper, IOrderViewClient orderClient, IKenticoResourceService kentico)
        {
            _mapper = mapper;
            _orderClient = orderClient;
            _kentico = kentico;
            _pageCapacity = int.Parse(_kentico.GetSettingsKey("KDA_RecentOrdersPageCapacity"));
        }

        public async Task<OrderHead> GetHeaders()
        {
            var orderList = _mapper.Map<OrderList>(await GetOrders(1));
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
                    RowsOnPage = _pageCapacity,
                    PagesCount = (_pageCapacity > 0 ? orderList.TotalCount / _pageCapacity : 0) + 1
                }
            };
        }

        private async Task<OrderListDto> GetOrders(int pageNumber)
        {
            var url = _kentico.GetSettingsKey("KDA_OrdersBySiteUrl");
            var siteName = _kentico.GetKenticoSite().Name;
            var result = await _orderClient.GetOrders(url, siteName, pageNumber, _pageCapacity);
            return result.Payload;
        }
    }
}