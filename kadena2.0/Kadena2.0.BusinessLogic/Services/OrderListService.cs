using Kadena.BusinessLogic.Contracts;
using Kadena.Models.RecentOrders;
using System.Collections.Generic;
using AutoMapper;
using Kadena2.MicroserviceClients.Contracts;
using System.Threading.Tasks;
using Kadena.Models;
using System.Linq;
using Kadena.Dto.Order;
using Kadena.Dto.General;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System;
using Kadena2.WebAPI.KenticoProviders.Contracts;
using Kadena.Models.Checkout;

namespace Kadena.BusinessLogic.Services
{
    public class OrderListService : IOrderListService
    {
        private readonly IMapper _mapper;
        private readonly IOrderViewClient _orderClient;
        private readonly IKenticoUserProvider _kenticoUsers;
        private readonly IKenticoResourceService _kenticoResources;
        private readonly IKenticoSiteProvider _site;
        private readonly IKenticoOrderProvider _order;
        private readonly IShoppingCartProvider _shoppingCart;
        private readonly IKenticoPermissionsProvider _permissions;
        private readonly IKenticoLogger _logger;
        private readonly IKenticoAddressBookProvider _kenticoAddressBook;

        private readonly string _orderDetailUrl;

        private int _pageCapacity;
        private string _pageCapacityKey;
        private Dictionary<int, string> _addressBookList;

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

        public OrderListService(IMapper mapper, IOrderViewClient orderClient, IKenticoUserProvider kenticoUsers,
            IKenticoResourceService kenticoResources, IKenticoSiteProvider site, IKenticoOrderProvider order,
            IShoppingCartProvider shoppingCart, IKenticoDocumentProvider documents, IKenticoPermissionsProvider permissions, IKenticoLogger logger, IKenticoAddressBookProvider kenticoAddressBook)
        {
            if (mapper == null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }
            if (orderClient == null)
            {
                throw new ArgumentNullException(nameof(orderClient));
            }
            if (kenticoUsers == null)
            {
                throw new ArgumentNullException(nameof(kenticoUsers));
            }
            if (kenticoResources == null)
            {
                throw new ArgumentNullException(nameof(kenticoResources));
            }
            if (site == null)
            {
                throw new ArgumentNullException(nameof(site));
            }
            if (order == null)
            {
                throw new ArgumentNullException(nameof(order));
            }
            if (shoppingCart == null)
            {
                throw new ArgumentNullException(nameof(shoppingCart));
            }
            if (permissions == null)
            {
                throw new ArgumentNullException(nameof(permissions));
            }
            if (documents == null)
            {
                throw new ArgumentNullException(nameof(documents));
            }
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }
            if (kenticoAddressBook == null)
            {
                throw new ArgumentNullException(nameof(kenticoAddressBook));
            }

            _mapper = mapper;
            _orderClient = orderClient;
            _kenticoUsers = kenticoUsers;
            _kenticoResources = kenticoResources;
            _site = site;
            _order = order;
            _shoppingCart = shoppingCart;
            _permissions = permissions;
            _logger = logger;
            _kenticoAddressBook = kenticoAddressBook;

            _orderDetailUrl = documents.GetDocumentUrl(kenticoResources.GetSettingsKey("KDA_OrderDetailUrl"));
        }

        public async Task<OrderHead> GetHeaders()
        {
            var orderList = _mapper.Map<OrderList>(await GetOrders(1));
            MapOrdersStatusToGeneric(orderList?.Orders);
            int pages = 0;
            if (EnablePaging && _pageCapacity > 0)
            {
                pages = orderList.TotalCount / _pageCapacity + (orderList.TotalCount % _pageCapacity > 0 ? 1 : 0);
            }
            return new OrderHead
            {
                Headings = new List<string> {
                    _kenticoResources.GetResourceString("Kadena.OrdersList.OrderNumber"),
                    _kenticoResources.GetResourceString("Kadena.OrdersList.OrderDate"),
                    _kenticoResources.GetResourceString("Kadena.OrdersList.OrderedItems"),
                    _kenticoResources.GetResourceString("Kadena.OrdersList.OrderStatus"),
                    _kenticoResources.GetResourceString("Kadena.OrdersList.ShippingDate"),
                    string.Empty
                },
                PageInfo = new Pagination
                {
                    RowsCount = orderList.TotalCount,
                    RowsOnPage = _pageCapacity,
                    PagesCount = pages
                },
                NoOrdersMessage = _kenticoResources.GetResourceString("Kadena.OrdersList.NoOrderItems"),
                Rows = orderList.Orders.Select(o =>
                {
                    o.ViewBtn = new Button { Text = _kenticoResources.GetResourceString("Kadena.OrdersList.View"), Url = $"{_orderDetailUrl}?orderID={o.Id}" };
                    return o;
                })
            };
        }

        public async Task<OrderBody> GetBody(int pageNumber)
        {
            var orderList = _mapper.Map<OrderList>(await GetOrders(pageNumber));
            MapOrdersStatusToGeneric(orderList?.Orders);
            return new OrderBody
            {
                Rows = orderList.Orders.Select(o =>
                {
                    o.ViewBtn = new Button { Text = _kenticoResources.GetResourceString("Kadena.OrdersList.View"), Url = $"{_orderDetailUrl}?orderID={o.Id}" };
                    return o;
                })
            };
        }

        private void MapOrdersStatusToGeneric(IEnumerable<Order> orders)
        {
            if (orders == null)
                return;

            foreach (var o in orders)
            {
                o.Status = _order.MapOrderStatus(o.Status);
            }
        }


        private async Task<OrderListDto> GetOrders(int pageNumber)
        {
            var siteName = _site.GetKenticoSite().Name;
            BaseResponseDto<OrderListDto> response = null;
            if (_permissions.IsAuthorizedPerResource("Kadena_Orders", "KDA_SeeAllOrders", siteName))
            {
                response = await _orderClient.GetOrders(siteName, pageNumber, _pageCapacity);
            }
            else
            {
                var customer = _kenticoUsers.GetCurrentCustomer();
                response = await _orderClient.GetOrders(customer?.Id ?? 0, pageNumber, _pageCapacity);
            }

            if (response?.Success ?? false)
            {
                return response.Payload;
            }
            else
            {
                _logger.LogError("OrderListService - GetOrders", response?.Error?.Message);
                return new OrderListDto();
            }
        }

        private async Task<OrderListDto> GetOrders(int pageNumber, int campaignID, string orderType)
        {
            var siteName = _site.GetKenticoSite().Name;
            BaseResponseDto<OrderListDto> response = null;
            if (_permissions.IsAuthorizedPerResource("Kadena_Orders", "KDA_SeeAllOrders", siteName))
            {
                response = await _orderClient.GetOrders(siteName, pageNumber, _pageCapacity, campaignID, orderType);
            }
            else
            {
                var customer = _kenticoUsers.GetCurrentCustomer();
                response = await _orderClient.GetOrders(customer?.Id ?? 0, pageNumber, _pageCapacity, campaignID, orderType);
            }

            if (response?.Success ?? false)
            {
                return response.Payload;
            }
            else
            {
                _logger.LogError("OrderListService - GetOrders", response?.Error?.Message);
                return new OrderListDto();
            }
        }

        public async Task<OrderHeadBlock> GetHeaders(string orderType, int campaignID)
        {
            _addressBookList = _kenticoAddressBook.GetAddressNames();
            var orderList = _mapper.Map<OrderList>(await GetOrders(1, campaignID, orderType));
            MapOrdersStatusToGeneric(orderList?.Orders);
            int pages = 0;
            if (EnablePaging && _pageCapacity > 0)
            {
                pages = orderList.TotalCount / _pageCapacity + (orderList.TotalCount % _pageCapacity > 0 ? 1 : 0);
            }
            return new OrderHeadBlock
            {
                headers = new List<string> {
                    _kenticoResources.GetResourceString("Kadena.OrdersList.OrderNumber"),
                    _kenticoResources.GetResourceString("Kadena.OrdersList.OrderDate"),
                    _kenticoResources.GetResourceString("Kadena.OrdersList.Distributor"),
                    _kenticoResources.GetResourceString("Kadena.OrdersList.OrderStatus"),
                    _kenticoResources.GetResourceString("Kadena.OrdersList.ShippingDate"),
                    string.Empty
                },
                PageInfo = new Pagination
                {
                    RowsCount = orderList.TotalCount,
                    RowsOnPage = _pageCapacity,
                    PagesCount = pages
                },
                NoOrdersMessage = _kenticoResources.GetResourceString("Kadena.OrdersList.NoOrderItems"),
                rows = orderList.Orders.Select(o =>
                {
                    return new OrderRow()
                    {
                        dailog = GetOrderDialog(o),
                        items = GetOrderItems(o)
                    };
                })
            };
        }

        private OrderDialog GetOrderDialog(Order o)
        {
            return new OrderDialog()
            {
                distributor = new OrderDailogLabel()
                {
                    label = _kenticoResources.GetResourceString("Kadena.OrdersList.Dailog.Distributor"),
                    value = GetDistributorName(o.campaign.DistributorID)
                },
                orderId = new OrderDailogLabel()
                {
                    label = _kenticoResources.GetResourceString("Kadena.OrdersList.Dailog.OrderID"),
                    value = o.Id
                },
                pdf = new OrderDailogLabel()
                {
                    label = _kenticoResources.GetResourceString("Kadena.OrdersList.Dailog.PDFBtnText"),
                    value = "#"
                },
                table = new OrderDialogTable()
                {
                    headers = new List<string>()
                        {
                            _kenticoResources.GetResourceString("Kadena.OrdersList.Dailog.POSNumber"),
                            _kenticoResources.GetResourceString("Kadena.OrdersList.Dailog.ProductName"),
                            _kenticoResources.GetResourceString("Kadena.OrdersList.Dailog.Quantity"),
                            _kenticoResources.GetResourceString("Kadena.OrdersList.Dailog.Price"),
                        },
                    rows = GetOrderItemsForDailog(o.Items)
                }
            };
        }

        private List<OrderTableCell> GetOrderItems(Order o)
        {
            return new List<OrderTableCell>()
                {
                    new OrderTableCell() { value = o.Id },
                    new OrderTableCell() { value = o.CreateDate.ToShortDateString() },
                    new OrderTableCell() { value = GetDistributorName(o.campaign.DistributorID), type = "longtext" },
                    new OrderTableCell() { value = o.Status, type = "hover-hide" },
                    new OrderTableCell() { value = o.ShippingDate.ToString(), type = "hover-hide" },
                    new OrderTableCell() { value = _kenticoResources.GetResourceString("Kadena.OrdersList.View"), type = "link", url = $"{_orderDetailUrl}?orderID={o.Id}" }
                };
        }

        private List<List<OrderDialogTableCell>> GetOrderItemsForDailog(IEnumerable<CartItem> items)
        {
            return items.Select(i =>
            {
                return new List<OrderDialogTableCell>()
                  {
                      new OrderDialogTableCell() { value = i.SKUNumber ?? string.Empty, span = 1 },
                      new OrderDialogTableCell() { value = i.SKUName, span = 1 },
                      new OrderDialogTableCell() { value = i.Quantity.ToString(), span = 1 },
                      new OrderDialogTableCell() { value = i.UnitPrice.ToString(), span = 1 }
                  };
            }).ToList();
        }

        private string GetDistributorName(int distributorID)
        {
            return _addressBookList.Where(x => x.Key.Equals(distributorID)).FirstOrDefault().Value ?? string.Empty;
        }
    }
}