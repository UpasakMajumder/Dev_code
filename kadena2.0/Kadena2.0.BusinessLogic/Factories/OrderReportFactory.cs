using AutoMapper;
using Kadena.BusinessLogic.Contracts;
using Kadena.Dto.Order;
using Kadena.Models;
using Kadena.Models.Common;
using Kadena.Models.Orders;
using Kadena.Models.SiteSettings;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.WebAPI.KenticoProviders.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using TableRow = Kadena.Models.Common.TableRow;
using Kadena.Models.Shipping;

namespace Kadena.BusinessLogic.Factories
{
    public interface IOrderReportFactory
    {
        /// <summary>
        /// Map typed report view to generic table view
        /// </summary>
        /// <param name="reportDto"></param>
        /// <returns></returns>
        TableView CreateTableView(IEnumerable<OrderReportViewItem> reportDto);

        IEnumerable<OrderReportViewItem> CreateReportView(IEnumerable<RecentOrderDto> recentOrder);
    }

    public class OrderReportFactory : IOrderReportFactory
    {
        private readonly IKenticoOrderProvider _kenticoOrderProvider;
        private readonly IKenticoCustomerProvider _kenticoCustomerProvider;
        private readonly IDateTimeFormatter _dateTimeFormatter;
        private readonly IKenticoResourceService _kenticoResources;
        private readonly IKenticoDocumentProvider _kenticoDocumentProvider;
        private readonly IOrderReportFactoryHeaders _orderReportFactoryHeaders;
        private readonly IMapper _mapper;
        private string _orderDetailUrl = string.Empty;

        public string OrderDetailUrl
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_orderDetailUrl))
                {
                    var defaultUrl = _kenticoResources.GetSiteSettingsKey(Settings.KDA_OrderDetailUrl);
                    _orderDetailUrl = _kenticoDocumentProvider.GetDocumentUrl(defaultUrl);
                }
                return _orderDetailUrl;
            }
        }

        public OrderReportFactory(IKenticoOrderProvider kenticoOrderProvider,
            IKenticoCustomerProvider kenticoCustomerProvider,
            IDateTimeFormatter dateTimeFormatter,
            IKenticoResourceService kenticoResources,
            IKenticoDocumentProvider kenticoDocumentProvider,
            IOrderReportFactoryHeaders orderReportFactoryHeaders,
            IMapper mapper)
        {
            _kenticoOrderProvider = kenticoOrderProvider ?? throw new ArgumentNullException(nameof(kenticoOrderProvider));
            _kenticoCustomerProvider = kenticoCustomerProvider ?? throw new ArgumentNullException(nameof(kenticoCustomerProvider));
            _dateTimeFormatter = dateTimeFormatter ?? throw new ArgumentNullException(nameof(dateTimeFormatter));
            _kenticoResources = kenticoResources ?? throw new ArgumentNullException(nameof(kenticoResources));
            _kenticoDocumentProvider = kenticoDocumentProvider ?? throw new ArgumentNullException(nameof(kenticoDocumentProvider));
            _orderReportFactoryHeaders = orderReportFactoryHeaders ?? throw new ArgumentNullException(nameof(orderReportFactoryHeaders));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public IEnumerable<OrderReportViewItem> CreateReportView(IEnumerable<RecentOrderDto> recentOrder) =>
            recentOrder
                .SelectMany(r => r.Items
                    .Select(i =>
                    {
                        var res = _mapper.Map<OrderReportViewItem>(i) ?? new OrderReportViewItem();
                        _mapper.Map(r, res);
                        res.Url = FormatDetailUrl(r);
                        res.OrderingDate = FormatDate(r.CreateDate);
                        res.User = FormatCustomer(_kenticoCustomerProvider.GetCustomer(r.ClientId));
                        res.Status = FormatOrderStatus(r.Status);
                        res.ShippingDate = FormatDate(i.ShippingDate);
                        res.Price = FormatPrice(i.UnitPrice * i.Quantity);
                        return res;
                    })
                );


        public TableView CreateTableView(IEnumerable<OrderReportViewItem> reportDto)
        {
            var orderReportViewItems = reportDto as OrderReportViewItem[] ?? reportDto.ToArray();

            var resultItems = new List<TableRow>();
            foreach (var orderReportViewItem in orderReportViewItems.Where(r => r.Quantity > 0))
            {
                if (orderReportViewItem.TrackingInfos.Any())
                {
                    var itemClone = _mapper.Map<OrderReportViewItem>(orderReportViewItem);
                    foreach (var trackingInfo in orderReportViewItem.TrackingInfos)
                    {
                        itemClone.TrackingInfos = new[] { trackingInfo };
                        resultItems.Add(_mapper.Map<TableRow>(itemClone));
                    }
                }
                else
                {
                    resultItems.Add(_mapper.Map<TableRow>(orderReportViewItem));
                }
            }

            return new TableView
            {
                Rows = resultItems.ToArray(),
                Headers = _orderReportFactoryHeaders.GetDisplayNameHeaders()
            };
        }

        private string FormatCustomer(Customer customer)
        {
            if (customer == null)
            {
                return string.Empty;
            }

            if (!string.IsNullOrWhiteSpace(customer.FullName))
            {
                return customer.FullName;
            }

            return customer.Email;
        }

        private string FormatDetailUrl(RecentOrderDto order) =>
            $"{OrderDetailUrl}?orderID={order.Id}";

        private string FormatOrderStatus(string status) =>
            _kenticoOrderProvider.MapOrderStatus(status);

        private string FormatDate(DateTime? date) =>
            date.HasValue
                ? _dateTimeFormatter.Format(date.Value)
                : string.Empty;

        private decimal FormatPrice(decimal price) =>
            Math.Round(price, 2);
    }
}
