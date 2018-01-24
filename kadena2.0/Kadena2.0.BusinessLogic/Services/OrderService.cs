using AutoMapper;
using Kadena.BusinessLogic.Contracts;
using Kadena.Dto.SubmitOrder.MicroserviceRequests;
using Kadena.Models;
using Kadena.Models.Checkout;
using Kadena.Models.OrderDetail;
using Kadena.Models.Product;
using Kadena.Models.SubmitOrder;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.MicroserviceClients.Contracts;
using Kadena2.WebAPI.KenticoProviders.Contracts;
using Kadena2.WebAPI.KenticoProviders.Contracts.KadenaSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Threading.Tasks;

namespace Kadena.BusinessLogic.Services
{
    public class OrderService : IOrderService
    {
        private readonly IMapper mapper;
        private readonly IKenticoOrderProvider kenticoOrder;
        private readonly IShoppingCartProvider shoppingCart;
        private readonly IKenticoProductsProvider products;
        private readonly IKenticoUserProvider kenticoUsers;
        private readonly IKenticoResourceService resources;
        private readonly IKenticoLogger kenticoLog;
        private readonly IOrderSubmitClient orderSubmitClient;
        private readonly IOrderViewClient orderViewClient;
        private readonly IMailingListClient mailingClient;
        private readonly ITaxEstimationService taxService;
        private readonly ITemplatedClient templateService;
        private readonly IKenticoDocumentProvider documents;
        private readonly IKenticoLocalizationProvider localization;
        private readonly IKenticoPermissionsProvider permissions;
        private readonly IKenticoSiteProvider siteProvider;
        private readonly IKadenaSettings settings;
        private readonly IKenticoBusinessUnitsProvider businessUnits;

        public OrderService(IMapper mapper,
            IOrderSubmitClient orderSubmitClient,
            IOrderViewClient orderViewClient,
            IMailingListClient mailingClient,
            IKenticoOrderProvider kenticoOrder,
            IShoppingCartProvider shoppingCart,
            IKenticoProductsProvider products,
            IKenticoUserProvider kenticoUsers,
            IKenticoResourceService resources,
            IKenticoLogger kenticoLog,
            ITaxEstimationService taxService,
            ITemplatedClient templateService,
            IKenticoDocumentProvider documents,
            IKenticoLocalizationProvider localization,
            IKenticoPermissionsProvider permissions,
            IKenticoSiteProvider site,
            IKadenaSettings settings,
            IKenticoBusinessUnitsProvider businessUnits)
        {
            if (mapper == null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }
            if (orderSubmitClient == null)
            {
                throw new ArgumentNullException(nameof(orderSubmitClient));
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
            if (taxService == null)
            {
                throw new ArgumentNullException(nameof(taxService));
            }
            if (templateService == null)
            {
                throw new ArgumentNullException(nameof(templateService));
            }
            if (documents == null)
            {
                throw new ArgumentNullException(nameof(documents));
            }
            if (localization == null)
            {
                throw new ArgumentNullException(nameof(localization));
            }
            if (permissions == null)
            {
                throw new ArgumentNullException(nameof(permissions));
            }
            if (site == null)
            {
                throw new ArgumentNullException(nameof(site));
            }
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }
            if (businessUnits == null)
            {
                throw new ArgumentNullException(nameof(businessUnits));
            }

            this.mapper = mapper;
            this.kenticoOrder = kenticoOrder;
            this.shoppingCart = shoppingCart;
            this.products = products;
            this.kenticoUsers = kenticoUsers;
            this.resources = resources;
            this.orderSubmitClient = orderSubmitClient;
            this.orderViewClient = orderViewClient;
            this.mailingClient = mailingClient;
            this.kenticoLog = kenticoLog;
            this.taxService = taxService;
            this.templateService = templateService;
            this.documents = documents;
            this.localization = localization;
            this.permissions = permissions;
            this.siteProvider = site;
            this.settings = settings;
            this.businessUnits = businessUnits;
        }

        public async Task<OrderDetail> GetOrderDetail(string orderId)
        {
            CheckOrderDetailPermisson(orderId, kenticoUsers.GetCurrentCustomer());

            var microserviceResponse = await orderViewClient.GetOrderByOrderId(orderId);

            if (!microserviceResponse.Success || microserviceResponse.Payload == null)
            {
                kenticoLog.LogError("GetOrderDetail", microserviceResponse.ErrorMessages);
                throw new Exception("Failed to obtain order detail from microservice"); // TODO refactor using checking null
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
                    Date = null, // TODO payment date unknown yet
                    PaidBy = data.PaymentInfo.PaymentMethod,
                    PaymentDetail = string.Empty,
                    PaymentIcon = GetPaymentMethodIcon(data.PaymentInfo.PaymentMethod),
                    Title = resources.GetResourceString("Kadena.Order.PaymentSection"),
                    DatePrefix = resources.GetResourceString("Kadena.Order.PaymentDatePrefix"),
                    BUnitLabel = resources.GetResourceString("Kadena.Order.BusinessUnitLabel"),
                    BUnitName = businessUnits.GetDistributorBusinessUnit(data.campaign != null ? data.campaign.DistributorID : 0)
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
                    Items = await MapOrderedItems(data.Items)
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
                    Tracking = null // TODO Track your package url unknown
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

        private async Task<List<OrderedItem>> MapOrderedItems(List<Dto.ViewOrder.MicroserviceResponses.OrderItemDTO> items)
        {
            var orderedItems = items.Select(i => new OrderedItem()
            {
                Id = i.SkuId,
                Image = products.GetSkuImageUrl(i.SkuId),
                MailingList = i.MailingList == Guid.Empty.ToString() ? string.Empty : i.MailingList,
                Price = String.Format("$ {0:#,0.00}", i.TotalPrice),
                Quantity = i.Quantity,
                QuantityShipped = i.QuantityShipped,
                QuantityPrefix = (i.Type ?? string.Empty).Contains("Mailing") ? resources.GetResourceString("Kadena.Order.QuantityPrefixAddresses") : resources.GetResourceString("Kadena.Order.QuantityPrefix"),
                QuantityShippedPrefix = resources.GetResourceString("Kadena.Order.QuantityShippedPrefix"),
                ShippingDate = string.Empty, // TODO Shipping date per item unknown
                Template = i.Name,
                TrackingId = i.TrackingId,
                MailingListPrefix = resources.GetResourceString("Kadena.Order.MailingListPrefix"),
                ShippingDatePrefix = resources.GetResourceString("Kadena.Order.ItemShippingDatePrefix"),
                TemplatePrefix = resources.GetResourceString("Kadena.Order.TemplatePrefix"),
                TrackingIdPrefix = resources.GetResourceString("Kadena.Order.TrackingIdPrefix"),
                ProductStatusPrefix = resources.GetResourceString("Kadena.Order.ProductStatusPrefix"),
                ProductStatus = products.GetProductStatus(i.SkuId)
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

        public async Task<SubmitOrderResult> SubmitOrder(SubmitOrderRequest request)
        {
            var paymentMethods = shoppingCart.GetPaymentMethods();
            var selectedPayment = paymentMethods.FirstOrDefault(p => p.Id == (request.PaymentMethod?.Id ?? -1));

            switch (selectedPayment?.ClassName ?? string.Empty)
            {
                case "KDA.PaymentMethods.CreditCard":
                    return await PayByCard(request);

                case "KDA.PaymentMethods.PurchaseOrder":
                case "KDA.PaymentMethods.MonthlyPayment":
                case "NoPaymentRequired":
                    return await SubmitPOOrder(request);

                case "KDA.PaymentMethods.PayPal":
                    throw new NotImplementedException("PayPal payment is not implemented yet");

                default:
                    throw new ArgumentOutOfRangeException("payment", "Unknown payment method");
            }
        }

        public async Task<SubmitOrderResult> PayByCard(SubmitOrderRequest request)
        {
            var insertCardUrl = resources.GetSettingsKey("KDA_CreditCard_InsertCardDetailsURL");

            return await Task.FromResult(new SubmitOrderResult
            {
                Success = true,
                RedirectURL = documents.GetDocumentUrl(insertCardUrl)
            }
            );
        }

        public async Task<SubmitOrderResult> SubmitPOOrder(SubmitOrderRequest request)
        {
            string serviceEndpoint = resources.GetSettingsKey("KDA_OrderServiceEndpoint");
            Customer customer = kenticoUsers.GetCurrentCustomer();

            var emails = request.EmailConfirmation.Union(new[] { customer.Email });

            if ((request?.DeliveryAddress?.Id ?? 0) < 0)
            {
                shoppingCart.SetShoppingCartAddress(request.DeliveryAddress);
                customer.FirstName = request.DeliveryAddress.CustomerName;
                customer.LastName = string.Empty;
                customer.Email = request.DeliveryAddress.Email;
                customer.Phone = request.DeliveryAddress.Phone;
            }


            var orderData = await GetSubmitOrderData(customer, request.DeliveryMethod, request.PaymentMethod.Id,
                request.PaymentMethod.Invoice, request.AgreeWithTandC, emails);

            if ((orderData?.Items?.Count() ?? 0) <= 0)
            {
                throw new ArgumentOutOfRangeException("Items", "Cannot submit order without items");
            }

            var serviceResultDto = await orderSubmitClient.SubmitOrder(orderData);
            var serviceResult = mapper.Map<SubmitOrderResult>(serviceResultDto);

            var redirectUrlBase = resources.GetSettingsKey("KDA_OrderSubmittedUrl");
            var redirectUrlBaseLocalized = documents.GetDocumentUrl(redirectUrlBase);
            var redirectUrl = $"{redirectUrlBaseLocalized}?success={serviceResult.Success}".ToLower();
            if (serviceResult.Success)
            {
                redirectUrl += "&order_id=" + serviceResult.Payload;
            }
            serviceResult.RedirectURL = redirectUrl;

            if (serviceResult.Success)
            {
                kenticoLog.LogInfo("Submit order", "INFORMATION", $"Order {serviceResult.Payload} successfully created");
                shoppingCart.RemoveCurrentItemsFromStock();
                shoppingCart.ClearCart();
            }
            else
            {
                kenticoLog.LogError("Submit order", $"Order {serviceResult?.Payload} error. {serviceResult?.Error?.Message}");
            }

            return serviceResult;
        }

        private async Task<Guid> CallRunGeneratePdfTask(CartItem cartItem)
        {
            var response = await templateService.RunGeneratePdfTask(cartItem.EditorTemplateId.ToString(), cartItem.ProductChiliPdfGeneratorSettingsId.ToString());
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

        private async Task<OrderDTO> GetSubmitOrderData(Customer customer, int deliveryMethodId, int paymentMethodId, string invoice,
            bool termsAndConditionsExplicitlyAccepted, IEnumerable<string> notificationEmails)
        {
            // TODO: add to order request. need confirmation on the name of the property from microservice side.

            var shippingAddress = shoppingCart.GetCurrentCartShippingAddress();
            shippingAddress.Country = localization.GetCountries().FirstOrDefault(c => c.Id == shippingAddress.Country.Id);
            shippingAddress.State = localization.GetStates().FirstOrDefault(c => c.Id == shippingAddress.State.Id);
            var billingAddress = shoppingCart.GetDefaultBillingAddress();
            var billingState = localization.GetStates().FirstOrDefault(c => c.Id == billingAddress.StateId);
            var site = siteProvider.GetKenticoSite();
            var paymentMethod = shoppingCart.GetPaymentMethod(paymentMethodId);
            var cartItems = shoppingCart.GetShoppingCartItems();
            var currency = siteProvider.GetSiteCurrency();
            var totals = shoppingCart.GetShoppingCartTotals();
            totals.TotalTax = await taxService.EstimateTotalTax(shippingAddress);

            if (string.IsNullOrWhiteSpace(customer.Company))
            {
                customer.Company = settings.DefaultCustomerCompanyName;
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
                    StateDisplayName = billingState?.StateDisplayName,
                    KenticoStateID = billingAddress.StateId,
                    KenticoCountryID = billingAddress.CountryId,
                    AddressCompanyName = settings.DefaultSiteCompanyName,
                    isoCountryCode = billingAddress.Country,
                    AddressPersonalName = settings.DefaultSitePersonalName,
                    Zip = billingAddress.Zip,
                    Country = billingAddress.Country,
                    KenticoAddressID = 0
                },
                ShippingAddress = new AddressDTO()
                {
                    AddressLine1 = shippingAddress.Address1,
                    AddressLine2 = shippingAddress.Address2,
                    City = shippingAddress.City,
                    State = !string.IsNullOrEmpty(shippingAddress.State?.StateCode) ? shippingAddress.State.StateCode : shippingAddress.Country.Name, // fill in mandatory for countries that have no states
                    StateDisplayName = shippingAddress.State?.StateDisplayName,
                    KenticoStateID = shippingAddress.State.Id,
                    KenticoCountryID = shippingAddress.Country.Id,
                    AddressCompanyName = customer.Company,
                    isoCountryCode = shippingAddress.Country.Code,
                    AddressPersonalName = $"{customer.FirstName} {customer.LastName}",
                    Zip = shippingAddress.Zip,
                    Country = shippingAddress.Country.Name,
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
                    KenticoOrderStatusID = kenticoOrder.GetOrderStatusId("Pending"),
                    OrderStatusName = "PENDING"
                },
                OrderTracking = new OrderTrackingDTO()
                {
                    OrderTrackingNumber = ""
                },
                TotalPrice = totals.TotalItemsPrice,
                TotalShipping = totals.TotalShipping,
                TotalTax = totals.TotalTax,
                Items = cartItems.Select(item => MapCartItemTypeToOrderItemType(item)),
                NotificationsData = notificationEmails.Select(e => new NotificationInfoDto
                {
                    Email = e,
                    Language = customer.PreferredLanguage
                })
            };

            // If only mailing list items in cart, we are not picking any delivery option
            if (!cartItems.All(i => i.IsMailingList))
            {
                var deliveryMethod = shoppingCart.GetShippingOption(deliveryMethodId);
                orderDto.ShippingOption = new ShippingOptionDTO()
                {
                    KenticoShippingOptionID = deliveryMethod.Id,
                    CarrierCode = deliveryMethod.SAPName,
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

        private string GetPaymentMethodIcon(string paymentMethod)
        {
            var methods = shoppingCart.GetPaymentMethods();
            var method = methods.FirstOrDefault(m => m.Title == paymentMethod);
            return method?.Icon ?? string.Empty;
        }
    }
}