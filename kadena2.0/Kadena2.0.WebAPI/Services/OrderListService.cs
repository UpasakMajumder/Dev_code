﻿using Kadena.WebAPI.Contracts;
using Kadena.WebAPI.Models.RecentOrders;
using System.Collections.Generic;
using AutoMapper;
using Kadena2.MicroserviceClients.Contracts;
using System.Threading.Tasks;
using Kadena.WebAPI.Models;
using System;
using System.Linq;
using Kadena.Dto.Order;
using Kadena.Dto.General;

namespace Kadena.WebAPI.Services
{
    public class OrderListService : IOrderListService
    {
        private readonly IMapper _mapper;
        private readonly IOrderViewClient _orderClient;
        private readonly IKenticoResourceService _kenticoResources;
        private readonly IKenticoProviderService _kentico;
        private readonly IKenticoLogger _logger;

        private int _pageCapacity;
        private string _pageCapacityKey;

        public string PageCapacityKey
        {
            get
            {
                return _pageCapacityKey;
            }

            set
            {
                _pageCapacityKey = value;
                string settingValue = _kenticoResources?.GetSettingsKey(_pageCapacityKey);
                _pageCapacity = int.Parse(string.IsNullOrWhiteSpace(settingValue) ? "5" : settingValue);
            }
        }

        public bool EnablePaging { get; set; }

        public OrderListService(IMapper mapper, IOrderViewClient orderClient,
            IKenticoResourceService kenticoResources, IKenticoProviderService kentico,
            IKenticoLogger logger)
        {
            _mapper = mapper;
            _orderClient = orderClient;
            _kenticoResources = kenticoResources;
            _kentico = kentico;
            _logger = logger;
        }

        public async Task<OrderHead> GetHeaders()
        {
            var orderList = _mapper.Map<OrderList>(await GetOrders(1));
            return new OrderHead
            {
                Headings = new List<string> {
                    _kenticoResources.GetResourceString("Kadena.OrdersList.OrderNumber"),
                    _kenticoResources.GetResourceString("Kadena.OrdersList.OrderDate"),
                    _kenticoResources.GetResourceString("Kadena.OrdersList.OrderedItems"),
                    _kenticoResources.GetResourceString("Kadena.OrdersList.OrderStatus"),
                    _kenticoResources.GetResourceString("Kadena.OrdersList.DeliveryDate"),
                    string.Empty
                },
                PageInfo = new Pagination
                {
                    RowsCount = orderList.TotalCount,
                    RowsOnPage = _pageCapacity,
                    PagesCount = EnablePaging ? (_pageCapacity > 0 ? orderList.TotalCount / _pageCapacity : 0) : 0 + 1
                },
                NoOrdersMessage = _kenticoResources.GetResourceString("Kadena.OrdersList.NoOrderItems")
            };
        }

        public async Task<OrderBody> GetBody(int pageNumber)
        {
            var orderDetailUrl = _kenticoResources.GetSettingsKey("KDA_OrderDetailUrl");
            var orderList = _mapper.Map<OrderList>(await GetOrders(pageNumber));
            return new OrderBody
            {
                Rows = orderList.Orders.Select(o =>
                {
                    o.ViewBtn = new Button { Text = _kenticoResources.GetResourceString("Kadena.OrdersList.View"), Url = $"{orderDetailUrl}?orderID={o.Id}" };
                    return o;
                })
            };
        }

        private async Task<OrderListDto> GetOrders(int pageNumber)
        {
            var siteName = _kenticoResources.GetKenticoSite().Name;
            BaseResponseDto<OrderListDto> result = null;
            if (_kentico.IsAuthorizedPerResource("Kadena_Orders", "KDA_SeeAllOrders", siteName))
            {
                var url = _kenticoResources.GetSettingsKey("KDA_OrdersBySiteUrl");
                result = await _orderClient.GetOrders(url, siteName, pageNumber, _pageCapacity);

            }
            else
            {
                var customer = _kentico.GetCurrentCustomer();
                var url = _kenticoResources.GetSettingsKey("KDA_OrderHistoryServiceEndpoint");
                result = await _orderClient.GetOrders(url, customer?.Id ?? 0, pageNumber, _pageCapacity);
            }

            if (result?.Success ?? false)
            {
                return result.Payload;
            }
            else
            {
                var exc = new InvalidOperationException(result?.Error?.Message);
                _logger.LogException("RecentOrdersService - GetOrders", exc);
                throw exc;
            }
        }
    }
}