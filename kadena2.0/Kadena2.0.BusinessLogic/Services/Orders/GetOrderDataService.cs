using AutoMapper;
using Kadena.BusinessLogic.Contracts;
using Kadena.BusinessLogic.Factories.Checkout;
using Kadena.Dto.SubmitOrder.MicroserviceRequests;
using Kadena.Models;
using Kadena.Models.Checkout;
using Kadena.Models.Product;
using Kadena.Models.SubmitOrder;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.BusinessLogic.Contracts.Orders;
using Kadena2.WebAPI.KenticoProviders.Contracts;
using Kadena2.WebAPI.KenticoProviders.Contracts.KadenaSettings;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Kadena2.BusinessLogic.Services.Orders
{
    public class GetOrderDataService : IGetOrderDataService
    {
        private readonly IMapper mapper;
        private readonly IKenticoOrderProvider kenticoOrder;
        private readonly IShoppingCartProvider shoppingCart;
        private readonly IShoppingCartItemsProvider shoppingCartItems;
        private readonly IKenticoUserProvider kenticoUsers;
        private readonly IKenticoLogger kenticoLog;
        private readonly ITaxEstimationService taxService;
        private readonly IKenticoSiteProvider siteProvider;
        private readonly IKadenaSettings settings;
        private readonly IOrderDataFactory orderDataFactory;

        public GetOrderDataService(IMapper mapper,
           IKenticoOrderProvider kenticoOrder,
           IShoppingCartProvider shoppingCart,
           IShoppingCartItemsProvider shoppingCartItems,
           IKenticoUserProvider kenticoUsers,
           IKenticoLogger kenticoLog,
           ITaxEstimationService taxService,
           IKenticoSiteProvider site,
           IKadenaSettings settings,
           IOrderDataFactory orderDataFactory
           )
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.kenticoOrder = kenticoOrder ?? throw new ArgumentNullException(nameof(kenticoOrder));
            this.shoppingCart = shoppingCart ?? throw new ArgumentNullException(nameof(shoppingCart));
            this.shoppingCartItems = shoppingCartItems ?? throw new ArgumentNullException(nameof(shoppingCartItems));
            this.kenticoUsers = kenticoUsers ?? throw new ArgumentNullException(nameof(kenticoUsers));
            this.kenticoLog = kenticoLog ?? throw new ArgumentNullException(nameof(kenticoLog));
            this.taxService = taxService ?? throw new ArgumentNullException(nameof(taxService));
            this.siteProvider = site ?? throw new ArgumentNullException(nameof(site));
            this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
            this.orderDataFactory = orderDataFactory ?? throw new ArgumentNullException(nameof(orderDataFactory));
        }

        public async Task<OrderDTO> GetSubmitOrderData(SubmitOrderRequest request)
        {
            Customer customer = kenticoUsers.GetCurrentCustomer();

            var notificationEmails = request.EmailConfirmation.Union(new[] { customer.Email });

            var shippingAddress = shoppingCart.GetCurrentCartShippingAddress();
            var billingAddress = shoppingCart.GetDefaultBillingAddress();
            var site = siteProvider.GetKenticoSite();
            var paymentMethod = shoppingCart.GetPaymentMethod(request.PaymentMethod.Id);
            var cartItems = shoppingCartItems.GetShoppingCartItems();
            var currency = siteProvider.GetSiteCurrency();
            var totals = shoppingCart.GetShoppingCartTotals();
            totals.TotalTax = await taxService.EstimateTotalTax(shippingAddress);

            if (string.IsNullOrWhiteSpace(customer.Company))
            {
                customer.Company = settings.DefaultCustomerCompanyName;
            }

            var orderDto = new OrderDTO()
            {
                BillingAddress = orderDataFactory.CreateBillingAddress(billingAddress),
                ShippingAddress = orderDataFactory.CreateShippingAddress(shippingAddress, customer),
                Customer = orderDataFactory.CreateCustomer(customer),
                OrderDate = DateTime.Now,
                PaymentOption = orderDataFactory.CreatePaymentOption(paymentMethod, request),
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
