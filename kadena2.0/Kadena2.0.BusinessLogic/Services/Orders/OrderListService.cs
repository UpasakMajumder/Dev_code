using AutoMapper;
using Kadena.BusinessLogic.Contracts;
using Kadena.Dto.Order;
using Kadena.Models;
using Kadena.Models.Checkout;
using Kadena.Models.Common;
using Kadena.Models.Orders;
using Kadena.Models.RecentOrders;
using Kadena.Models.SiteSettings;
using Kadena.Models.SiteSettings.Permissions;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.MicroserviceClients.Contracts;
using Kadena2.WebAPI.KenticoProviders.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kadena.BusinessLogic.Services.Orders
{
    public class OrderListService : IOrderListService
    {
        private readonly IMapper _mapper;
        private readonly IOrderViewClient _orderClient;
        private readonly IKenticoCustomerProvider _kenticoCustomers;
        private readonly IKenticoResourceService _kenticoResources;
        private readonly IKenticoSiteProvider _site;
        private readonly IKenticoOrderProvider _order;
        private readonly IKenticoPermissionsProvider _permissions;
        private readonly IKenticoLogger _logger;
        private readonly IKenticoAddressBookProvider _kenticoAddressBook;
        private readonly IKenticoDocumentProvider _documents;
        private readonly IKenticoCustomerProvider _customers;

        private string _orderDetailUrl = string.Empty;
        public string OrderDetailUrl
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_orderDetailUrl))
                {
                    var defaultUrl = _kenticoResources.GetSiteSettingsKey(Settings.KDA_OrderDetailUrl);
                    _orderDetailUrl = _documents.GetDocumentUrl(defaultUrl);
                }
                return _orderDetailUrl;
            }
        }

        public string PageCapacityKey { get; set; }

        public bool EnablePaging { get; set; }

        private Dictionary<int, string> _customerDictionary = new Dictionary<int, string>();

        public OrderListService(IMapper mapper,
                                IOrderViewClient orderClient,
                                IKenticoCustomerProvider kenticoCustomers,
                                IKenticoResourceService kenticoResources,
                                IKenticoSiteProvider site,
                                IKenticoOrderProvider order,
                                IKenticoDocumentProvider documents,
                                IKenticoPermissionsProvider permissions,
                                IKenticoLogger logger,
                                IKenticoCustomerProvider customers,
                                IKenticoAddressBookProvider kenticoAddressBook)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _orderClient = orderClient ?? throw new ArgumentNullException(nameof(orderClient));
            _kenticoCustomers = kenticoCustomers ?? throw new ArgumentNullException(nameof(kenticoCustomers));
            _kenticoResources = kenticoResources ?? throw new ArgumentNullException(nameof(kenticoResources));
            _site = site ?? throw new ArgumentNullException(nameof(site));
            _order = order ?? throw new ArgumentNullException(nameof(order));
            _permissions = permissions ?? throw new ArgumentNullException(nameof(permissions));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _kenticoAddressBook = kenticoAddressBook ?? throw new ArgumentNullException(nameof(kenticoAddressBook));
            _documents = documents ?? throw new ArgumentNullException(nameof(documents));
            _customers = customers ?? throw new ArgumentNullException(nameof(customers));
        }

        public async Task<OrderHead> GetOrdersToApprove()
        {
            var postFilter = GetApproverFilter();

            var filter = CreateFilterForOrdersToApprove();
            var orderHeaders = await GetHeaders(filter, postFilter);
            orderHeaders.PageInfo = Pagination.Empty;

            return orderHeaders;
        }

        private Func<OrderListDto, OrderListDto> GetApproverFilter()
        {
            var siteName = _site.GetCurrentSiteCodeName();
            var isApprover = _permissions.CurrentUserHasPermission(
                ModulePermissions.KadenaOrdersModule,
                ModulePermissions.KadenaOrdersModule.ApproveOrders,
                siteName);
            if (!isApprover)
            {
                throw new UnauthorizedAccessException(
                    $"Access denied. Missing permission '{ModulePermissions.KadenaOrdersModule.ApproveOrders}'");
            }

            OrderListDto WhereCurrentUserIsApprover(OrderListDto orders)
            {
                var currentUser = _kenticoCustomers.GetCurrentCustomer();
                var approvingCustomers = _kenticoCustomers
                    .GetCustomersByApprover(currentUser.UserID);

                if (approvingCustomers == null || orders?.Orders == null)
                {
                    return orders;
                }

                orders.Orders = orders.Orders
                    .Where(o => approvingCustomers.Any(c => c.Id == o.ClientId))
                    .ToList();

                return orders;
            }

            return WhereCurrentUserIsApprover;
        }

        private OrderListFilter CreateFilterForOrdersToApprove()
        {
            var almostUnlimitedNumberOfItems = 10_000_000;
            var filter = new OrderListFilter
            {
                PageNumber = 1,
                ItemsPerPage = almostUnlimitedNumberOfItems,
                Status = (int)OrderStatus.WaitingForApproval
            };

            return filter;
        }

        private int CalculateNumberOfPages(int numberOfOrders)
        {
            var pageCapacity = _kenticoResources.GetSiteSettingsKey<int>(PageCapacityKey);
            return EnablePaging && pageCapacity > 0
                ? numberOfOrders / pageCapacity + (numberOfOrders % pageCapacity > 0 ? 1 : 0)
                : 0;
        }

        private Button BuildOrderDetailButton(Order order) => new Button
        {
            Exists = true,
            Text = _kenticoResources.GetResourceString("Kadena.OrdersList.View"),
            Url = $"{OrderDetailUrl}?orderID={order.Id}"
        };

        public Task<OrderHead> GetHeaders()
        {
            var filter = CreateFilterForRecentOrders(1);
            return GetHeaders(filter);
        }

        private async Task<OrderHead> GetHeaders(OrderListFilter filter, Func<OrderListDto, OrderListDto> afterFilter = null)
        {
            var orderList = await GetOrders(filter, afterFilter);
            var pages = CalculateNumberOfPages(orderList.TotalCount);
            var headings = new List<string>
            {
                _kenticoResources.GetResourceString("Kadena.OrdersList.OrderNumber"),
                _kenticoResources.GetResourceString("Kadena.OrdersList.OrderDate"),
                _kenticoResources.GetResourceString("Kadena.OrdersList.OrderedItems"),
                _kenticoResources.GetResourceString("Kadena.OrdersList.OrderStatus"),
                _kenticoResources.GetResourceString("Kadena.OrdersList.ShippingDate"),
                string.Empty
            };
            if (CanSeeAllOrders())
            {
                headings.Insert(headings.Count - 1, _kenticoResources.GetResourceString("Kadena.OrdersList.Customer"));
            }
            return new OrderHead
            {
                Headings = headings,
                PageInfo = new Pagination
                {
                    RowsCount = orderList.TotalCount,
                    RowsOnPage = orderList.Orders.Count(),
                    PagesCount = pages
                },
                NoOrdersMessage = _kenticoResources.GetResourceString("Kadena.OrdersList.NoOrderItems"),
                Rows = orderList.Orders.Select(o =>
                {
                    o.ViewBtn = BuildOrderDetailButton(o);
                    return o;
                })
            };
        }

        public async Task<OrderBody> GetBody(int pageNumber)
        {
            var filter = CreateFilterForRecentOrders(pageNumber);
            var orderList = await GetOrders(filter);

            return new OrderBody
            {
                Rows = orderList.Orders.Select(o =>
                {
                    o.ViewBtn = BuildOrderDetailButton(o);
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

        private bool CanSeeAllOrders() 
            => _permissions.CurrentUserHasPermission(ModulePermissions.KadenaOrdersModule, ModulePermissions.KadenaOrdersModule.SeeAllOrders);

        private OrderListFilter CreateFilterForRecentOrders(int pageNumber)
        {
            var filter = new OrderListFilter
            {
                PageNumber = pageNumber,
                ItemsPerPage = _kenticoResources.GetSiteSettingsKey<int>(PageCapacityKey)
            };

            if (CanSeeAllOrders())
            {
                filter.SiteName = _site.GetKenticoSite().Name;
            }
            else
            {
                var customer = _kenticoCustomers.GetCurrentCustomer();
                filter.CustomerId = customer?.Id;
            }

            return filter;
        }

        private async Task<OrderList> GetOrders(OrderListFilter filter, Func<OrderListDto, OrderListDto> afterFilter = null)
        {
            var response = await _orderClient.GetOrders(filter);

            var orders = new OrderListDto();

            if (response?.Success ?? false)
            {
                orders = response.Payload;
            }
            else
            {
                _logger.LogError("OrderListService - GetOrders", response?.ErrorMessages);
            }

            if (afterFilter != null)
            {
                orders = afterFilter(orders);
            }

            var orderList = _mapper.Map<OrderList>(orders);
            RemoveRemovedLineItems(orderList);
            MapOrdersStatusToGeneric(orderList?.Orders);
            AddCustomerData(orderList?.Orders);
            return orderList;
        }

        private void AddCustomerData(IEnumerable<Order> orders)
        {
            if (!CanSeeAllOrders())
            {
                return;
            }

            foreach (var item in orders)
            {
                item.ClientName = GetCustomerName(item.ClientId);
            }
        }

        private string GetCustomerName(int customerId)
        {
            if (!_customerDictionary.TryGetValue(customerId, out var customerName))
            {
                var customer = _customers.GetCustomer(customerId);
                if (customer != null)
                {
                    customerName = $"{customer.FirstName} {customer.LastName}".Trim();
                }
                else
                {
                    customerName = "";
                }

                _customerDictionary[customerId] = customerName;
            }

            return customerName;
        }

        private void RemoveRemovedLineItems(OrderList orderList)
        {
            foreach (var order in orderList.Orders)
            {
                order.Items = order.Items.Where(it => it.Quantity > 0);
            }
        }

        public async Task<OrderHeadBlock> GetCampaignHeaders(string orderType, int campaignID)
        {
            var filter = CreateFilterForRecentOrders(pageNumber: 1);
            filter.CampaignId = campaignID;
            filter.OrderType = orderType;
            return await GetCampaignHeaders(filter);
        }

        private async Task<OrderHeadBlock> GetCampaignHeaders(OrderListFilter filter, Func<OrderListDto, OrderListDto> afterFilter = null)
        {
            var orderList = await GetOrders(filter, afterFilter);

            var pages = CalculateNumberOfPages(orderList.TotalCount);
            var headers = new List<string>
            {
                _kenticoResources.GetResourceString("Kadena.OrdersList.OrderNumber"),
                _kenticoResources.GetResourceString("Kadena.OrdersList.OrderDate"),
                _kenticoResources.GetResourceString("Kadena.OrdersList.Distributor"),
                _kenticoResources.GetResourceString("Kadena.OrdersList.OrderStatus"),
                _kenticoResources.GetResourceString("Kadena.OrdersList.ShippingDate"),
                string.Empty
            };
            if (CanSeeAllOrders())
            {
                headers.Insert(headers.Count - 1, _kenticoResources.GetResourceString("Kadena.OrdersList.Customer"));
            }
            return new OrderHeadBlock
            {
                headers = headers,
                PageInfo = new Pagination
                {
                    RowsCount = orderList.TotalCount,
                    RowsOnPage = _kenticoResources.GetSiteSettingsKey<int>(PageCapacityKey),
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
                    rows = GetOrderItemsForDialog(o.Items)
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
                    new OrderTableCell() { value = o.ClientName, type = "hover-hide" },
                    new OrderTableCell() { value = _kenticoResources.GetResourceString("Kadena.OrdersList.View"), type = "link", url = $"{OrderDetailUrl}?orderID={o.Id}" }
                };
        }

        private List<List<OrderDialogTableCell>> GetOrderItemsForDialog(IEnumerable<CheckoutCartItem> items)
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
            var _addressBookList = _kenticoAddressBook.GetAddressNames();
            return _addressBookList.TryGetValue(distributorID, out var name) ? name : string.Empty;
        }

        public async Task<OrderHeadBlock> GetCampaignOrdersToApprove(string orderType, int campaignID)
        {
            var postFilter = GetApproverFilter();

            var filter = CreateFilterForOrdersToApprove();
            filter.CampaignId = campaignID;
            filter.OrderType = orderType;
            var orderHeaders = await GetCampaignHeaders(filter, postFilter);
            orderHeaders.PageInfo = Pagination.Empty;
            return orderHeaders;
        }
    }
}