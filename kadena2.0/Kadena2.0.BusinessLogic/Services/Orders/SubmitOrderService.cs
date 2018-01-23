using AutoMapper;
using Kadena.BusinessLogic.Contracts;
using Kadena.Dto.SubmitOrder.MicroserviceRequests;
using Kadena.Models;
using Kadena.Models.Checkout;
using Kadena.Models.Product;
using Kadena.Models.SubmitOrder;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.BusinessLogic.Contracts.OrderPayment;
using Kadena2.MicroserviceClients.Contracts;
using Kadena2.WebAPI.KenticoProviders.Contracts;
using Kadena2.WebAPI.KenticoProviders.Contracts.KadenaSettings;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Kadena.BusinessLogic.Services.Orders
{
    public class SubmitOrderService : ISubmitOrderService
    {
        private readonly IMapper mapper;
        private readonly IKenticoOrderProvider kenticoOrder;
        private readonly IShoppingCartProvider shoppingCart;
        private readonly IKenticoUserProvider kenticoUsers;
        private readonly IKenticoLogger kenticoLog;
        private readonly ITaxEstimationService taxService;
        private readonly ITemplatedClient templateService;
        private readonly IKenticoLocalizationProvider localization;
        private readonly IKenticoSiteProvider siteProvider;
        private readonly IKadenaSettings settings;
        private readonly ICreditCard3dsi creditCard3dsi;
        private readonly IPurchaseOrder purchaseOrder;

        public SubmitOrderService(IMapper mapper,
            IKenticoOrderProvider kenticoOrder,
            IShoppingCartProvider shoppingCart,
            IKenticoUserProvider kenticoUsers,
            IKenticoLogger kenticoLog,
            ITaxEstimationService taxService,
            ITemplatedClient templateService,
            IKenticoLocalizationProvider localization,
            IKenticoSiteProvider site,
            IKadenaSettings settings,
            ICreditCard3dsi creditCard3dsi,
            IPurchaseOrder purchaseOrder
            )
        {
            if (mapper == null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }
            if (kenticoOrder == null)
            {
                throw new ArgumentNullException(nameof(kenticoOrder));
            }
            if (shoppingCart == null)
            {
                throw new ArgumentNullException(nameof(shoppingCart));
            }
            if (kenticoUsers == null)
            {
                throw new ArgumentNullException(nameof(kenticoUsers));
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
            if (localization == null)
            {
                throw new ArgumentNullException(nameof(localization));
            }
            if (site == null)
            {
                throw new ArgumentNullException(nameof(site));
            }
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }
            if (creditCard3dsi == null)
            {
                throw new ArgumentNullException(nameof(creditCard3dsi));
            }
            if (purchaseOrder == null)
            {
                throw new ArgumentNullException(nameof(purchaseOrder));
            }

            this.mapper = mapper;
            this.kenticoOrder = kenticoOrder;
            this.shoppingCart = shoppingCart;
            this.kenticoUsers = kenticoUsers;
            this.kenticoLog = kenticoLog;
            this.taxService = taxService;
            this.templateService = templateService;
            this.localization = localization;
            this.siteProvider = site;
            this.settings = settings;
            this.creditCard3dsi = creditCard3dsi;
            this.purchaseOrder = purchaseOrder;
        }

        public async Task<SubmitOrderResult> SubmitOrder(SubmitOrderRequest request)
        {
            var paymentMethods = shoppingCart.GetPaymentMethods();
            var selectedPayment = paymentMethods.FirstOrDefault(p => p.Id == (request.PaymentMethod?.Id ?? -1));
            var orderData = await GetSubmitOrderData(request);

            switch (selectedPayment?.ClassName ?? string.Empty)
            {
                case "KDA.PaymentMethods.CreditCard":
                    return await creditCard3dsi.PayByCard3dsi(orderData);

                case "KDA.PaymentMethods.PurchaseOrder":
                case "KDA.PaymentMethods.MonthlyPayment":
                case "NoPaymentRequired":
                    return await purchaseOrder.SubmitPOOrder(orderData);

                case "KDA.PaymentMethods.PayPal":
                    throw new NotImplementedException("PayPal payment is not implemented yet");

                default:
                    throw new ArgumentOutOfRangeException("payment", "Unknown payment method");
            }
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

        private async Task<OrderDTO> GetSubmitOrderData(SubmitOrderRequest request)
        {
            // TODO: add to order request. need confirmation on the name of the property from microservice side.

            Customer customer = kenticoUsers.GetCurrentCustomer();

            var notificationEmails = request.EmailConfirmation.Union(new[] { customer.Email });

            if ((request?.DeliveryAddress?.Id ?? 0) < 0)
            {
                shoppingCart.SetShoppingCartAddress(request.DeliveryAddress);
                customer.FirstName = request.DeliveryAddress.CustomerName;
                customer.LastName = string.Empty;
                customer.Email = request.DeliveryAddress.Email;
                customer.Phone = request.DeliveryAddress.Phone;
            }

            var shippingAddress = shoppingCart.GetCurrentCartShippingAddress();
            shippingAddress.Country = localization.GetCountries().FirstOrDefault(c => c.Id == shippingAddress.Country.Id);
            shippingAddress.State = localization.GetStates().FirstOrDefault(c => c.Id == shippingAddress.State.Id);
            var billingAddress = shoppingCart.GetDefaultBillingAddress();
            var billingState = localization.GetStates().FirstOrDefault(c => c.Id == billingAddress.StateId);
            var site = siteProvider.GetKenticoSite();
            var paymentMethod = shoppingCart.GetPaymentMethod(request.PaymentMethod.Id);
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
                    PONumber = request.PaymentMethod.Invoice
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
                var deliveryMethod = shoppingCart.GetShippingOption(request.DeliveryMethod);
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
    }
}