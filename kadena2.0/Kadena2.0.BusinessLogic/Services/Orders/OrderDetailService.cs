using AutoMapper;
using Kadena.BusinessLogic.Contracts;
using Kadena.Dto.SubmitOrder.MicroserviceRequests;
using Kadena.Helpers;
using Kadena.Models;
using Kadena.Models.Checkout;
using Kadena.Models.Common;
using Kadena.Models.OrderDetail;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.MicroserviceClients.Contracts;
using Kadena2.WebAPI.KenticoProviders.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Threading.Tasks;

namespace Kadena.BusinessLogic.Services.Orders
{
    public class OrderDetailService : IOrderDetailService
    {
        private readonly IMapper mapper;
        private readonly IOrderViewClient orderViewClient;
        private readonly IMailingListClient mailingClient;
        private readonly IKenticoOrderProvider kenticoOrder;
        private readonly IShoppingCartProvider shoppingCart;
        private readonly IKenticoProductsProvider products;
        private readonly IKenticoUserProvider kenticoUsers;
        private readonly IKenticoResourceService resources;
        private readonly IKenticoLogger kenticoLog;
        private readonly IKenticoLocalizationProvider localization;
        private readonly IKenticoPermissionsProvider permissions;
        private readonly IKenticoBusinessUnitsProvider businessUnits;


        public OrderDetailService(IMapper mapper,
            IOrderViewClient orderViewClient,
            IMailingListClient mailingClient,
            IKenticoOrderProvider kenticoOrder,
            IShoppingCartProvider shoppingCart,
            IKenticoProductsProvider products,
            IKenticoUserProvider kenticoUsers,
            IKenticoResourceService resources,
            IKenticoLogger kenticoLog,
            IKenticoLocalizationProvider localization,
            IKenticoPermissionsProvider permissions,
            IKenticoBusinessUnitsProvider businessUnits
            )
        {
            if (mapper == null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }
            if (orderViewClient == null)
            {
                throw new ArgumentNullException(nameof(orderViewClient));
            }
            if (mailingClient == null)
            {
                throw new ArgumentNullException(nameof(mailingClient));
            }
            if (kenticoOrder == null)
            {
                throw new ArgumentNullException(nameof(kenticoOrder));
            }
            if (shoppingCart == null)
            {
                throw new ArgumentNullException(nameof(shoppingCart));
            }
            if (products == null)
            {
                throw new ArgumentNullException(nameof(products));
            }
            if (kenticoUsers == null)
            {
                throw new ArgumentNullException(nameof(kenticoUsers));
            }
            if (resources == null)
            {
                throw new ArgumentNullException(nameof(resources));
            }
            if (kenticoLog == null)
            {
                throw new ArgumentNullException(nameof(kenticoLog));
            }
            if (localization == null)
            {
                throw new ArgumentNullException(nameof(localization));
            }
            if (permissions == null)
            {
                throw new ArgumentNullException(nameof(permissions));
            }
            if (businessUnits == null)
            {
                throw new ArgumentNullException(nameof(businessUnits));
            }

            this.mapper = mapper;
            this.orderViewClient = orderViewClient;
            this.kenticoOrder = kenticoOrder;
            this.shoppingCart = shoppingCart;
            this.products = products;
            this.kenticoUsers = kenticoUsers;
            this.resources = resources;
            this.mailingClient = mailingClient;
            this.kenticoLog = kenticoLog;
            this.localization = localization;
            this.permissions = permissions;
            this.businessUnits = businessUnits;
        }

        public async Task<OrderDetail> GetOrderDetail(string orderId)
        {
            CheckOrderDetailPermisson(orderId, kenticoUsers.GetCurrentCustomer());

            var microserviceResponse = await orderViewClient.GetOrderByOrderId(orderId);

            if (!microserviceResponse.Success || microserviceResponse.Payload == null)
            {
                kenticoLog.LogError("GetOrderDetail", microserviceResponse.ErrorMessages);
                return null;
            }

            var data = microserviceResponse.Payload;
            var genericStatus = kenticoOrder.MapOrderStatus(data.Status);

            var orderDetail = new OrderDetail()
            {
                DateTimeNAString = resources.GetResourceString("Kadena.Order.ItemShippingDateNA"),

                CommonInfo = new CommonInfo()
                {
                    OrderDate = new TitleValuePair<DateTime>
                    {
                        Title = resources.GetResourceString("Kadena.Order.OrderDateTitle"),
                        Value = data.OrderDate
                    },
                    ShippingDate = new TitleValuePair<DateTime?>
                    {
                        Title = resources.GetResourceString("Kadena.Order.ShippingDatePrefix"),
                        Value = data.ShippingInfo?.ShippingDate
                    },
                    Status = new TitleValuePair<string>
                    {
                        Title = resources.GetResourceString("Kadena.Order.StatusPrefix"),
                        Value = genericStatus
                    },
                    TotalCost = new TitleValuePair<string>
                    {
                        Title = resources.GetResourceString("Kadena.Order.TotalCostPrefix"),
                        Value = String.Format("$ {0:#,0.00}", data.PaymentInfo.Summary + data.PaymentInfo.Shipping + data.PaymentInfo.Tax)
                    }
                },
                PaymentInfo = new PaymentInfo()
                {
                    Date = data.PaymentInfo.CapturedDate,
                    PaidBy = data.PaymentInfo.PaymentMethod,
                    PaymentDetail = string.Empty,
                    PaymentIcon = GetPaymentMethodIcon(data.PaymentInfo.PaymentMethod),
                    Title = resources.GetResourceString("Kadena.Order.PaymentSection"),
                    DatePrefix = resources.GetResourceString("Kadena.Order.PaymentDatePrefix"),
                    BUnitLabel = resources.GetResourceString("Kadena.Order.BusinessUnitLabel"),
                    BUnitName = businessUnits.GetBusinessUnitName(data.campaign != null ? data.campaign.BusinessUnitNumber : string.Empty)
                },
                PricingInfo = new PricingInfo()
                {
                    Title = resources.GetResourceString("Kadena.Order.PricingSection"),
                    Items = new List<PricingInfoItem>()
                    {
                        new PricingInfoItem()
                        {
                            Title = resources.GetResourceString("Kadena.Order.PricingSummary"),
                            Value = String.Format("$ {0:#,0.00}", data.PaymentInfo.Summary)
                        },
                        new PricingInfoItem()
                        {
                            Title = resources.GetResourceString("Kadena.Order.PricingShipping"),
                            Value = String.Format("$ {0:#,0.00}", data.PaymentInfo.Shipping)
                        },
                        new PricingInfoItem()
                        {
                            Title = resources.GetResourceString("Kadena.Order.PricingSubtotal"),
                            Value = String.Format("$ {0:#,0.00}",data.PaymentInfo.Summary + data.PaymentInfo.Shipping)
                        },
                        new PricingInfoItem()
                        {
                            Title = resources.GetResourceString("Kadena.Order.PricingTax"),
                            Value = String.Format("$ {0:#,0.00}",data.PaymentInfo.Tax)
                        },
                        new PricingInfoItem()
                        {
                            Title = resources.GetResourceString("Kadena.Order.PricingTotals"),
                            Value = String.Format("$ {0:#,0.00}",data.PaymentInfo.Summary + data.PaymentInfo.Shipping + data.PaymentInfo.Tax)
                        }
                    }
                },
                OrderedItems = new OrderedItems()
                {
                    Title = resources.GetResourceString("Kadena.Order.OrderedItemsSection"),
                    Items = await MapOrderedItems(data.Items, data.Id)
                }
            };

            var mailingTypeCode = OrderItemTypeDTO.Mailing.ToString();
            var hasOnlyMailingListProducts = data.Items.All(item => item.Type == mailingTypeCode);
            if (hasOnlyMailingListProducts)
            {
                orderDetail.ShippingInfo = new ShippingInfo
                {
                    Title = resources.GetResourceString("Kadena.Order.ShippingSection"),
                    Message = resources.GetResourceString("Kadena.Checkout.UndeliverableText")
                };
            }
            else
            {
                orderDetail.ShippingInfo = new ShippingInfo
                {
                    Title = resources.GetResourceString("Kadena.Order.ShippingSection"),
                    DeliveryMethod = shoppingCart.GetShippingProviderIcon(data.ShippingInfo.Provider),
                    Address = mapper.Map<DeliveryAddress>(data.ShippingInfo.AddressTo),
                    Tracking = null
                };
                orderDetail.ShippingInfo.Address.Country = localization
                    .GetCountries()
                    .FirstOrDefault(s => s.Code.Equals(data.ShippingInfo.AddressTo.isoCountryCode));
            }

            if (!permissions.UserCanSeePrices())
            {
                orderDetail.HidePrices();
            }

            return orderDetail;
        }

        private async Task<List<OrderedItem>> MapOrderedItems(List<Dto.ViewOrder.MicroserviceResponses.OrderItemDTO> items, string orderId)
        {
            var orderedItems = items.Select(i =>
            {
                var product = i.TemplateId != Guid.Empty ? products.GetProductBySkuId(i.SkuId) : null;
                return new OrderedItem()
                {
                    Id = i.SkuId,
                    Image = products.GetSkuImageUrl(i.SkuId),
                    DownloadPdfURL = $"/api/pdf/hires/{orderId}/{i.LineNumber}",
                    MailingList = i.MailingList == Guid.Empty.ToString() ? string.Empty : i.MailingList,
                    Price = String.Format("$ {0:#,0.00}", i.TotalPrice),
                    Quantity = i.Quantity,
                    QuantityShipped = i.QuantityShipped,
                    QuantityPrefix = (i.Type ?? string.Empty).Contains("Mailing") ? resources.GetResourceString("Kadena.Order.QuantityPrefixAddresses") : resources.GetResourceString("Kadena.Order.QuantityPrefix"),
                    QuantityShippedPrefix = resources.GetResourceString("Kadena.Order.QuantityShippedPrefix"),
                    ShippingDate = string.Empty,
                    Template = i.Name,
                    TrackingId = i.TrackingId,
                    MailingListPrefix = resources.GetResourceString("Kadena.Order.MailingListPrefix"),
                    ShippingDatePrefix = resources.GetResourceString("Kadena.Order.ItemShippingDatePrefix"),
                    TemplatePrefix = resources.GetResourceString("Kadena.Order.TemplatePrefix"),
                    TrackingIdPrefix = resources.GetResourceString("Kadena.Order.TrackingIdPrefix"),
                    ProductStatusPrefix = resources.GetResourceString("Kadena.Order.ProductStatusPrefix"),
                    ProductStatus = products.GetProductStatus(i.SkuId),
                    Preview = new Button
                    {
                        Exists = product != null,
                        Text = resources.GetResourceString("Kadena.Checkout.PreviewButton"),
                        Url = UrlHelper.GetUrlForTemplatePreview(i.TemplateId, product?.TemplateLowResSettingId ?? Guid.Empty)
                    },
                    Options = i.Attributes?.Select(a => new ItemOption { Name = products.GetOptionCategory(a.Key)?.DisplayName ?? a.Key, Value = a.Value }) ?? Enumerable.Empty<ItemOption>()
                };
            }).ToList();


            await SetMailingListNames(orderedItems);

            return orderedItems;
        }

        private async Task SetMailingListNames(List<OrderedItem> orderedItems)
        {
            var mailingResponse = await mailingClient.GetMailingListsForCustomer();

            if (mailingResponse == null || mailingResponse.Success == false || mailingResponse.Payload == null)
            {
                kenticoLog.LogError("MailingList client", $"Call to microservice failed. {mailingResponse?.ErrorMessages}");
                return;
            }

            var mailingLists = mailingResponse.Payload;
            var itemsWithMailing = orderedItems.Where(i => !string.IsNullOrWhiteSpace(i.MailingList) && i.MailingList != Guid.Empty.ToString());

            foreach (var item in itemsWithMailing)
            {
                var matchingList = mailingLists.FirstOrDefault(m => m.Id == item.MailingList);

                if (matchingList != null)
                {
                    item.MailingList = matchingList.Name;
                }
            }
        }

        private void CheckOrderDetailPermisson(string orderId, Customer customer)
        {
            if (string.IsNullOrWhiteSpace(orderId))
            {
                throw new ArgumentNullException(nameof(orderId));
            }

            int orderUserId;
            int orderCustomerId;
            var orderIdparts = orderId.Split(new char[] { '-' }, 3);

            if (orderIdparts.Length != 3 || !int.TryParse(orderIdparts[0], out orderCustomerId) || !int.TryParse(orderIdparts[1], out orderUserId))
            {
                throw new ArgumentOutOfRangeException(nameof(orderId), "Bad format of customer ID");
            }

            // Allow admin who has set permission to see all orders in Kentico
            // or Allow orders belonging to currently logged User and Customer
            bool isAdmin = permissions.UserCanSeeAllOrders();
            bool isOrderOwner = (orderUserId == customer.UserID && orderCustomerId == customer.Id);
            if (isAdmin || isOrderOwner)
            {
                return;
            }

            throw new SecurityException("Permission denied");
        }

        private string GetPaymentMethodIcon(string paymentMethod)
        {
            var methods = shoppingCart.GetPaymentMethods();
            var method = methods.FirstOrDefault(m => m.Title == paymentMethod);
            return method?.Icon ?? string.Empty;
        }
    }
}