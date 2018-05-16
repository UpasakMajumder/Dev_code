using AutoMapper;
using Kadena.BusinessLogic.Contracts;
using Kadena.Dto.SubmitOrder.MicroserviceRequests;
using Kadena.Helpers;
using Kadena.Helpers.Routes;
using Kadena.Models;
using Kadena.Models.Checkout;
using Kadena.Models.Common;
using Kadena.Models.OrderDetail;
using Kadena.Models.Orders;
using Kadena.Models.Product;
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
        private readonly IKenticoCustomerProvider kenticoCustomers;
        private readonly IKenticoResourceService resources;
        private readonly IKenticoLogger kenticoLog;
        private readonly IKenticoLocalizationProvider localization;
        private readonly IKenticoPermissionsProvider permissions;
        private readonly IKenticoBusinessUnitsProvider businessUnits;
        private readonly IKenticoSiteProvider site;
        private readonly IImageService imageService;
        private readonly IPdfService pdfService;
        private readonly IKenticoUnitOfMeasureProvider units;

        public OrderDetailService(IMapper mapper,
            IOrderViewClient orderViewClient,
            IMailingListClient mailingClient,
            IKenticoOrderProvider kenticoOrder,
            IShoppingCartProvider shoppingCart,
            IKenticoProductsProvider products,
            IKenticoCustomerProvider kenticoCustomers,
            IKenticoResourceService resources,
            IKenticoLogger kenticoLog,
            IKenticoLocalizationProvider localization,
            IKenticoPermissionsProvider permissions,
            IKenticoBusinessUnitsProvider businessUnits,
            IKenticoSiteProvider site,
            IImageService imageService,
            IPdfService pdfService,
            IKenticoUnitOfMeasureProvider units
            )
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.orderViewClient = orderViewClient ?? throw new ArgumentNullException(nameof(orderViewClient));
            this.kenticoOrder = kenticoOrder ?? throw new ArgumentNullException(nameof(kenticoOrder));
            this.shoppingCart = shoppingCart ?? throw new ArgumentNullException(nameof(shoppingCart));
            this.products = products ?? throw new ArgumentNullException(nameof(products));
            this.kenticoCustomers = kenticoCustomers?? throw new ArgumentNullException(nameof(kenticoCustomers));
            this.resources = resources ?? throw new ArgumentNullException(nameof(resources));
            this.mailingClient = mailingClient ?? throw new ArgumentNullException(nameof(mailingClient));
            this.kenticoLog = kenticoLog ?? throw new ArgumentNullException(nameof(kenticoLog));
            this.localization = localization ?? throw new ArgumentNullException(nameof(localization));
            this.permissions = permissions ?? throw new ArgumentNullException(nameof(permissions));
            this.businessUnits = businessUnits ?? throw new ArgumentNullException(nameof(businessUnits));
            this.site = site ?? throw new ArgumentNullException(nameof(site));
            this.imageService = imageService ?? throw new ArgumentNullException(nameof(imageService));
            this.pdfService = pdfService ?? throw new ArgumentNullException(nameof(pdfService));
            this.units = units ?? throw new ArgumentNullException(nameof(units));
        }

        public async Task<OrderDetail> GetOrderDetail(string orderId)
        {
            CheckOrderDetailPermisson(orderId, kenticoCustomers.GetCurrentCustomer());

            var microserviceResponse = await orderViewClient.GetOrderByOrderId(orderId);

            if (!microserviceResponse.Success || microserviceResponse.Payload == null)
            {
                kenticoLog.LogError("GetOrderDetail", microserviceResponse.ErrorMessages);
                return null;
            }

            var data = microserviceResponse.Payload;
            var genericStatus = kenticoOrder.MapOrderStatus(data.Status);

            var businessUnitName = "";
            if (long.TryParse(data.campaign?.BusinessUnitNumber, out var bun))
            {
                businessUnitName = businessUnits.GetBusinessUnitName(bun);
            }

            var customer = kenticoCustomers.GetCustomer(data.ClientId) ?? Customer.Unknown;

            var isWaitingForApproval = data.StatusId == (int)OrderStatus.WaitingForApproval;

            var orderDetail = new OrderDetail()
            {
                DateTimeNAString = resources.GetResourceString("Kadena.Order.ItemShippingDateNA"),

                General = new OrderInfo
                {
                    OrderId = orderId,
                    CustomerId = customer.Id,
                    CustomerName = customer.FullName
                },

                Actions = isWaitingForApproval
                    ? new OrderActions
                    {
                        Accept = new DialogButton
                        {
                            Button = "Kadena.Order.ButtonAccept",
                            Dialog = new Dialog
                            {
                                CancelButton = "Kadena.Order.DialogAccept.Cancel",
                                ProceedButton = "Kadena.Order.DialogAccept.Proceed",
                                ProceedUrl = '/' + Routes.Order.Approve,
                                Text = "Kadena.Order.DialogAccept.Message",
                                Title = "Kadena.Order.DialogAccept.Title"
                            }
                        },
                        Reject = new DialogButton
                        {
                            Button = "Kadena.Order.ButtonReject",
                            Dialog = new Dialog
                            {
                                CancelButton = "Kadena.Order.DialogReject.Cancel",
                                ProceedButton = "Kadena.Order.DialogReject.Proceed",
                                ProceedUrl = '/' + Routes.Order.Reject,
                                Text = "Kadena.Order.DialogReject.Message",
                                Title = "Kadena.Order.DialogReject.Title"
                            }
                        }
                    }
                    : null,

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
                    BUnitName = businessUnitName
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

        private string GetPdfUrl(string orderId, Dto.ViewOrder.MicroserviceResponses.OrderItemDTO orderItem, Product orderedProduct)
        {
            if (orderItem.Type.Contains(OrderItemTypeDTO.TemplatedProduct.ToString()) ||
                orderItem.Type.Contains(OrderItemTypeDTO.Mailing.ToString()))
            {
                if (orderedProduct == null)
                {
                    kenticoLog.LogError("GetPdfUrl", $"Couldn't find product for item line {orderItem.LineNumber} from order {orderId}");
                    return string.Empty;
                }

                if (orderedProduct.HiResPdfDownloadEnabled)
                {
                    return pdfService.GetHiresPdfUrl(orderId, orderItem.LineNumber);
                }

                return pdfService.GetLowresPdfUrl(orderItem.TemplateId, orderedProduct.TemplateLowResSettingId);
            }

            return string.Empty;
        }

        private async Task<List<OrderedItem>> MapOrderedItems(List<Dto.ViewOrder.MicroserviceResponses.OrderItemDTO> items, string orderId)
        {
            var orderedItems = items.Select(i =>
            {
                var templatedProduct = i.TemplateId != Guid.Empty ? products.GetProductBySkuId(i.SkuId) : null;
                var previewUrl = UrlHelper.GetUrlForTemplatePreview(i.TemplateId, templatedProduct?.TemplateLowResSettingId ?? Guid.Empty);
                var previewAbsoluteUrl = site.GetAbsoluteUrl(previewUrl);

                return new OrderedItem()
                {
                    Id = i.SkuId,
                    Image = imageService.GetThumbnailLink(products.GetSkuImageUrl(i.SkuId)),
                    DownloadPdfURL =  GetPdfUrl(orderId, i, templatedProduct),
                    MailingList = i.MailingList == Guid.Empty.ToString() ? string.Empty : i.MailingList,
                    Price = String.Format("$ {0:#,0.00}", i.TotalPrice),
                    UnitOfMeasure = units.GetDisplaynameByCode(i.UnitOfMeasure),
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
                        Exists = templatedProduct != null,
                        Text = resources.GetResourceString("Kadena.Checkout.PreviewButton"),
                        Url = previewAbsoluteUrl
                    },
                    EmailProof = new Button
                    {
                        Exists = templatedProduct != null,
                        Text = resources.GetResourceString("Kadena.EmailProof.ButtonLabel"),
                        Url = GetPdfUrl(orderId, i, templatedProduct)
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