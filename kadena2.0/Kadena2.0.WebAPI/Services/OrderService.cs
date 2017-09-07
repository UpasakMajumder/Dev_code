using AutoMapper;
using Kadena.Dto.SubmitOrder.MicroserviceRequests;
using Kadena.WebAPI.Contracts;
using Kadena.Models;
using Kadena.Models.OrderDetail;
using Kadena.Models.SubmitOrder;
using Kadena2.MicroserviceClients.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena.Models.Checkout;

namespace Kadena.WebAPI.Services
{
    public class OrderService : IOrderService
    {
        private readonly IMapper mapper;
        private readonly IKenticoProviderService kenticoProvider;
        private readonly IKenticoUserProvider kenticoUsers;
        private readonly IKenticoResourceService resources;
        private readonly IKenticoLogger kenticoLog;
        private readonly IOrderSubmitClient orderSubmitClient;
        private readonly IOrderViewClient orderViewClient;
        private readonly IMailingListClient mailingClient;
        private readonly ITaxEstimationService taxService;
        private readonly ITemplatedProductService templateService;

        public OrderService(IMapper mapper,
            IOrderSubmitClient orderSubmitClient,
            IOrderViewClient orderViewClient,
            IMailingListClient mailingClient,
            IKenticoProviderService kenticoProvider,
            IKenticoUserProvider kenticoUsers,
            IKenticoResourceService resources,
            IKenticoLogger kenticoLog,
            ITaxEstimationService taxService,
            ITemplatedProductService templateService)
        {
            this.mapper = mapper;
            this.kenticoProvider = kenticoProvider;
            this.kenticoUsers = kenticoUsers;
            this.resources = resources;
            this.orderSubmitClient = orderSubmitClient;
            this.orderViewClient = orderViewClient;
            this.mailingClient = mailingClient;
            this.kenticoLog = kenticoLog;
            this.taxService = taxService;
            this.templateService = templateService;
        }

        public async Task<OrderDetail> GetOrderDetail(string orderId)
        {
            CheckOrderDetailPermisson(orderId, kenticoUsers.GetCurrentCustomer());

            var endpoint = resources.GetSettingsKey("KDA_OrderViewDetailServiceEndpoint");
            var microserviceResponse = await orderViewClient.GetOrderByOrderId(endpoint, orderId);

            if (!microserviceResponse.Success || microserviceResponse.Payload == null)
            {
                kenticoLog.LogError("GetOrderDetail", microserviceResponse.ErrorMessages);
                throw new Exception("Failed to obtain order detail from microservice"); // TODO refactor using checking null
            }

            var data = microserviceResponse.Payload;

            var orderDetail = new OrderDetail()
            {
                CommonInfo = new CommonInfo()
                {
                    OrderDate = data.OrderDate.ToString("MM/dd/yyyy"),
                    ShippingDate = CheckedDateTimeString(data.ShippingInfo?.ShippingDate ?? DateTime.MinValue),
                    Status = data.Status,
                    TotalCost = String.Format("$ {0:#,0.00}", data.PaymentInfo.Summary + data.PaymentInfo.Shipping + data.PaymentInfo.Tax)
                },
                PaymentInfo = new PaymentInfo()
                {
                    Date = CheckedDateTimeString(DateTime.MinValue), // TODO payment date unknown
                    PaidBy = data.PaymentInfo.PaymentMethod,
                    PaymentDetail = string.Empty,
                    PaymentIcon = GetPaymentMethodIcon(data.PaymentInfo.PaymentMethod),
                    Title = "Payment"
                },
                PricingInfo = new PricingInfo()
                {
                    Title = "Pricing",
                    Items = new List<PricingInfoItem>()
                    {
                        new PricingInfoItem()
                        {
                            Title = "Summary",
                            Value = String.Format("$ {0:#,0.00}",data.PaymentInfo.Summary)
                        },
                        new PricingInfoItem()
                        {
                            Title = "Shipping",
                            Value = String.Format("$ {0:#,0.00}",data.PaymentInfo.Shipping)
                        },
                        new PricingInfoItem()
                        {
                            Title = "Subtotal",
                            Value = String.Format("$ {0:#,0.00}",data.PaymentInfo.Summary + data.PaymentInfo.Shipping)
                        },
                        new PricingInfoItem()
                        {
                            Title = "Tax",
                            Value = String.Format("$ {0:#,0.00}",data.PaymentInfo.Tax)
                        },
                        new PricingInfoItem()
                        {
                            Title = "Totals",
                            Value = String.Format("$ {0:#,0.00}",data.PaymentInfo.Summary + data.PaymentInfo.Shipping + data.PaymentInfo.Tax)
                        }
                    }
                },
                OrderedItems = new OrderedItems()
                {
                    Title = "Ordered items",
                    Items = await MapOrderedItems(data.Items)
                }
            };

            var mailingTypeCode = OrderItemTypeDTO.Mailing.ToString();
            var hasOnlyMailingListProducts = data.Items.All(item => item.Type == mailingTypeCode);
            if (hasOnlyMailingListProducts)
            {
                orderDetail.ShippingInfo = new ShippingInfo
                {
                    Title = "Shipping",
                    Message = resources.GetResourceString("Kadena.Checkout.UndeliverableText")
                };
            }
            else
            {
                orderDetail.ShippingInfo = new ShippingInfo
                {
                    Title = "Shipping",
                    DeliveryMethod = kenticoProvider.GetShippingProviderIcon(data.ShippingInfo.Provider),
                    Address = data.ShippingInfo.AddressTo,
                    Tracking = null, // TODO Track your package url unknown
                    /*Tracking = new Tracking()
                    {
                        Text = "Track your packages",
                        Url = string.Empty 
                    }*/
                };
            }

            if (!kenticoUsers.UserCanSeePrices())
            {
                orderDetail.HidePrices();
            }


            return orderDetail;
        }

        private async Task<List<OrderedItem>> MapOrderedItems(List<Dto.ViewOrder.MicroserviceResponses.OrderItemDTO> items)
        {
            var orderedItems = items.Select(i => new OrderedItem()
            {
                Id = i.SkuId,
                // TODO Uncomment this when DownloadPDF will be developed.
                // DownloadPdfURL = (i.Type ?? string.Empty).ToLower().Contains("template") ? i.FileUrl : string.Empty,
                Image = kenticoProvider.GetSkuImageUrl(i.SkuId),
                MailingList = i.MailingList == Guid.Empty.ToString() ? string.Empty : i.MailingList,
                Price = String.Format("$ {0:#,0.00}", i.TotalPrice),
                Quantity = i.Quantity,
                QuantityShipped = i.QuantityShipped,
                QuantityPrefix = (i.Type ?? string.Empty).Contains("Mailing") ? resources.GetResourceString("Kadena.Order.QuantityPrefixAddresses") : resources.GetResourceString("Kadena.Order.QuantityPrefix"),
                QuantityShippedPrefix = resources.GetResourceString("Kadena.Order.QuantityShippedPrefix"),
                ShippingDate = string.Empty, // TODO Shipping date per item unknown
                Template = i.Name,
                TrackingId = i.TrackingId
            }).ToList();


            await SetMailingListNames(orderedItems);

            return orderedItems;
        }

        private async Task SetMailingListNames(List<OrderedItem> orderedItems)
        {
            var endpoint = resources.GetSettingsKey("KDA_GetMailingListsUrl");
            var customerName = resources.GetKenticoSite().Name;
            var mailingResponse = await mailingClient.GetMailingListsForCustomer(endpoint, customerName);

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
            bool isAdmin = kenticoUsers.UserCanSeeAllOrders();
            bool isOrderOwner = (orderUserId == customer.UserID && orderCustomerId == customer.Id);
            if (isAdmin || isOrderOwner)
            {
                return;
            }

            throw new SecurityException("Permission denied");
        }

        public async Task<SubmitOrderResult> SubmitOrder(SubmitOrderRequest request)
        {
            string serviceEndpoint = resources.GetSettingsKey("KDA_OrderServiceEndpoint");
            Customer customer = null;
            if ((request?.DeliveryAddress?.Id ?? 0) < 0)
            {
                kenticoProvider.SetShoppingCartAddress(request.DeliveryAddress);
                customer = new Customer
                {
                    FirstName = request.DeliveryAddress.CustomerName,
                    Email = request.DeliveryAddress.Email,
                    Phone = request.DeliveryAddress.Phone
                };
            }

            var orderData = await GetSubmitOrderData(customer, request.DeliveryMethod, request.PaymentMethod.Id, request.PaymentMethod.Invoice);

            if ((orderData?.Items?.Count() ?? 0) <= 0)
            {
                throw new ArgumentOutOfRangeException("Items", "Cannot submit order without items");
            }

            var serviceResultDto = await orderSubmitClient.SubmitOrder(serviceEndpoint, orderData);
            var serviceResult = mapper.Map<SubmitOrderResult>(serviceResultDto);
            var redirectUrl = $"/order-submitted?success={serviceResult.Success.ToString().ToLower()}";
            if (serviceResult.Success)
            {
                redirectUrl += "&order_id=" + serviceResult.Payload;
            }

            serviceResult.RedirectURL = redirectUrl;

            if (serviceResult.Success)
            {
                kenticoLog.LogInfo("Submit order", "INFORMATION", $"Order {serviceResult.Payload} successfully created");
                kenticoProvider.RemoveCurrentItemsFromStock();
                kenticoProvider.RemoveCurrentItemsFromCart();
            }
            else
            {
                kenticoLog.LogError("Submit order", $"Order {serviceResult?.Payload} error. {serviceResult?.Error?.Message}");
            }

            return serviceResult;
        }

        private async Task<Guid> CallRunGeneratePdfTask(CartItem cartItem)
        {
            string endpoint = resources.GetSettingsKey("KDA_TemplatingServiceEndpoint");
            var response = await templateService.RunGeneratePdfTask(endpoint, cartItem.EditorTemplateId.ToString(), cartItem.ProductChiliPdfGeneratorSettingsId.ToString());
            if (response.Success && response.Payload != null)
            {
                return new Guid(response.Payload.TaskId);
            }
            else
            {
                kenticoLog.LogError($"Call run generate PDF task",
                    $"Template service client with templateId = {cartItem.EditorTemplateId} and settingsId = {cartItem.ProductChiliPdfGeneratorSettingsId}" +
                    response?.Error?.Message ?? string.Empty);
            }
            return Guid.Empty;
        }

        private async Task<OrderDTO> GetSubmitOrderData(Customer customerInfo, int deliveryMethodId, int paymentMethodId, string invoice)
        {
            var customer = customerInfo ?? kenticoUsers.GetCurrentCustomer();
            var shippingAddress = kenticoProvider.GetCurrentCartShippingAddress();
            var billingAddress = kenticoProvider.GetDefaultBillingAddress();
            var site = resources.GetKenticoSite();
            var paymentMethod = kenticoProvider.GetPaymentMethod(paymentMethodId);
            var cartItems = kenticoProvider.GetShoppingCartItems();
            var currency = resources.GetSiteCurrency();
            var totals = kenticoProvider.GetShoppingCartTotals();
            totals.TotalTax = await taxService.EstimateTotalTax(shippingAddress);

            if (string.IsNullOrWhiteSpace(customer.Company))
            {
                customer.Company = resources.GetDefaultCustomerCompanyName();
            }

            foreach (var item in cartItems.Where(i => i.IsTemplated))
            {
                var taskId = await CallRunGeneratePdfTask(item);
                item.DesignFilePathTaskId = taskId;
            }

            var orderDto = new OrderDTO()
            {
                BillingAddress = new AddressDTO()
                {
                    AddressLine1 = billingAddress.Street.Count > 0 ? billingAddress.Street[0] : null,
                    AddressLine2 = billingAddress.Street.Count > 1 ? billingAddress.Street[1] : null,
                    City = billingAddress.City,
                    State = !string.IsNullOrEmpty(billingAddress.State) ? billingAddress.State : billingAddress.Country, // fill in mandatory for countries that have no states
                    KenticoStateID = billingAddress.StateId,
                    KenticoCountryID = billingAddress.CountryId,
                    AddressCompanyName = resources.GetDefaultSiteCompanyName(),
                    isoCountryCode = billingAddress.Country,
                    AddressPersonalName = resources.GetDefaultSitePersonalName(),
                    Zip = billingAddress.Zip,
                    Country = billingAddress.Country,
                    KenticoAddressID = 0
                },
                ShippingAddress = new AddressDTO()
                {
                    AddressLine1 = shippingAddress.Street.Count > 0 ? shippingAddress.Street[0] : null,
                    AddressLine2 = shippingAddress.Street.Count > 1 ? shippingAddress.Street[1] : null,
                    City = shippingAddress.City,
                    State = !string.IsNullOrEmpty(shippingAddress.State) ? shippingAddress.State : shippingAddress.Country, // fill in mandatory for countries that have no states
                    KenticoStateID = shippingAddress.StateId,
                    KenticoCountryID = shippingAddress.CountryId,
                    AddressCompanyName = customer.Company,
                    isoCountryCode = shippingAddress.Country,
                    AddressPersonalName = $"{customer.FirstName} {customer.LastName}",
                    Zip = shippingAddress.Zip,
                    Country = shippingAddress.Country,
                    KenticoAddressID = shippingAddress.Id
                },
                Customer = new CustomerDTO()
                {
                    CustomerNumber = customer.CustomerNumber,
                    Email = customer.Email,
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    KenticoCustomerID = customer.Id,
                    KenticoUserID = customer.UserID,
                    Phone = customer.Phone
                },
                KenticoOrderCreatedByUserID = customer.UserID,
                OrderDate = DateTime.Now,
                PaymentOption = new PaymentOptionDTO()
                {
                    KenticoPaymentOptionID = paymentMethod.Id,
                    PaymentOptionName = paymentMethod.Title,
                    PONumber = invoice
                },
                Site = new SiteDTO()
                {
                    KenticoSiteID = site.Id,
                    KenticoSiteName = site.Name,
                    ErpCustomerId = site.ErpCustomerId
                },
                OrderCurrency = new CurrencyDTO()
                {
                    CurrencyCode = currency.Code,
                    KenticoCurrencyID = currency.Id
                },
                OrderStatus = new OrderStatusDTO()
                {
                    KenticoOrderStatusID = resources.GetOrderStatusId("Pending"),
                    OrderStatusName = "PENDING"
                },
                OrderTracking = new OrderTrackingDTO()
                {
                    OrderTrackingNumber = ""
                },
                TotalPrice = totals.TotalItemsPrice,
                TotalShipping = totals.TotalShipping,
                TotalTax = totals.TotalTax,
                Items = cartItems.Select(item => MapCartItemTypeToOrderItemType(item))
            };

            // If only mailing list items in cart, we are not picking any delivery option
            if (!cartItems.All(i => i.IsMailingList))
            {
                var deliveryMethod = kenticoProvider.GetShippingOption(deliveryMethodId);
                orderDto.ShippingOption = new ShippingOptionDTO()
                {
                    KenticoShippingOptionID = deliveryMethod.Id,
                    CarrierCode = deliveryMethod.Title,
                    ShippingCompany = deliveryMethod.CarrierCode,
                    ShippingService = deliveryMethod.Service.Replace("#", "")
                };
            }

            return orderDto;
        }

        private OrderItemDTO MapCartItemTypeToOrderItemType(CartItem item)
        {
            var mappedItem = mapper.Map<OrderItemDTO>(item);
            mappedItem.Type = ConvertCartItemProductTypeToOrderItemProductType(item.ProductType);
            return mappedItem;
        }

        private OrderItemTypeDTO ConvertCartItemProductTypeToOrderItemProductType(string productType)
        {
            var cartItemFlags = productType.Split('|');

            var standardTypes = new[]
            {
                ProductTypes.POD, ProductTypes.StaticProduct, ProductTypes.InventoryProduct, ProductTypes.ProductWithAddOns
            };

            if (cartItemFlags.Contains(ProductTypes.MailingProduct))
            {
                return OrderItemTypeDTO.Mailing;
            }
            else if (cartItemFlags.Contains(ProductTypes.TemplatedProduct))
            {
                return OrderItemTypeDTO.TemplatedProduct;
            }
            else if (cartItemFlags.Any(flag => standardTypes.Contains(flag)))
            {
                return OrderItemTypeDTO.StandardOnStockItem;
            }
            else
            {
                throw new ArgumentException($"Missing mapping or invalid product type '{ productType }'");
            }
        }

        private string CheckedDateTimeString(DateTime dt)
        {
            return dt == DateTime.MinValue ? resources.GetResourceString("Kadena.Order.ItemShippingDateNA") : dt.ToString("MM/dd/yyyy");
        }

        private string GetPaymentMethodIcon(string paymentMethod)
        {
            var methods = kenticoProvider.GetPaymentMethods();
            var method = methods.FirstOrDefault(m => m.Title == paymentMethod);
            return method?.Icon ?? string.Empty;
        }
    }
}