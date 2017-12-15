using Kadena.BusinessLogic.Contracts;
using Kadena.Dto.General;
using Kadena.Dto.Order;
using Kadena.Helpers;
using Kadena.Models.Dashboard;
using Kadena.WebAPI.KenticoProviders;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.MicroserviceClients.Clients;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kadena.BusinessLogic.Services
{
    public class DashboardStatisticsService : IDashboardStatisticsService
    {
        private readonly IKenticoDashboardStatisticsProvider kenticoDashboardStatistics;
        private readonly IKenticoResourceService _kenticoResources;

        public DashboardStatisticsService(IKenticoDashboardStatisticsProvider kenticoDashboardStatistics, IKenticoResourceService kenticoResources)
        {
            this.kenticoDashboardStatistics = kenticoDashboardStatistics;
            this._kenticoResources = kenticoResources;
        }

        public DashboardStatistics GetDashboardStatistics()
        {
            DashboardStatistics statistics = new DashboardStatistics()
            {
                NewSalespersons = GetSalespersonStatistics()
            };
            var siteName = _kenticoResources.GetKenticoSite().Name;
            var statisticClient = new OrderViewClient(new MicroProperties(new KenticoResourceService()));
            BaseResponseDto<OrderListDto> response = statisticClient.GetOrders(siteName, 1, 1000).Result;
            if (response.Success)
            {
                statistics.OpenOrders = GetOrdersBlock(response.Payload.Orders, "Submission in progress");
                statistics.OrdersPlaced = GetOrdersBlock(response.Payload.Orders, "Submitted");
            }
            return statistics;
        }

        private StatisticBlock GetSalespersonStatistics()
        {
            return kenticoDashboardStatistics.GetSalespersonStatistics();
        }

        private StatisticBlock GetOrdersBlock(IEnumerable<OrderDto> ordersList, string orderStatus)
        {
            List<OrderDto> Weekly = FilterOrders(ordersList, orderStatus, 7);
            List<OrderDto> Monthly = FilterOrders(ordersList, orderStatus, 30);
            List<OrderDto> Yearly = FilterOrders(ordersList, orderStatus, 365);
            return new StatisticBlock()
            {
                Week = new StatisticsReading()
                {
                    Count = Weekly.Count,
                    Cost = Weekly.Sum(x => x.TotalPrice).ToString()
                },
                Month = new StatisticsReading()
                {
                    Count = Monthly.Count,
                    Cost = Monthly.Sum(x => x.TotalPrice).ToString()
                },
                Year = new StatisticsReading()
                {
                    Count = Yearly.Count,
                    Cost = Yearly.Sum(x => x.TotalPrice).ToString()
                }
            };
        }

        private List<OrderDto> FilterOrders(IEnumerable<OrderDto> ordersList, string orderStatus, int lastNDays)
        {
            return ordersList.Where(x => x.Status.Equals(orderStatus) && (DateTime.Now - x.CreateDate).Days <= lastNDays).ToList();
        }
    }
}
