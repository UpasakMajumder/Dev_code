using AutoMapper;
using Kadena.BusinessLogic.Contracts;
using Kadena.BusinessLogic.Contracts.Orders;
using Kadena.BusinessLogic.Factories.Checkout;
using Kadena.Dto.SubmitOrder.MicroserviceRequests;
using Kadena.Models;
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
        private readonly IOrderCartItemsProvider orderCartItems;
        private readonly IKenticoCustomerProvider kenticoCustomer;
        private readonly IKenticoLogger kenticoLog;
        private readonly ITaxEstimationService taxService;
        private readonly IKenticoSiteProvider siteProvider;
        private readonly IKadenaSettings settings;
        private readonly IOrderDataFactory orderDataFactory;
        private readonly IKenticoResourceService resources;
        private readonly IDeliveryEstimationDataService deliveryEstimationData;

        public GetOrderDataService(IMapper mapper,
           IKenticoOrderProvider kenticoOrder,
           IShoppingCartProvider shoppingCart,
           IOrderCartItemsProvider orderCartItems,
           IKenticoCustomerProvider kenticoCustomer,
           IKenticoLogger kenticoLog,
           ITaxEstimationService taxService,
           IKenticoSiteProvider site,
           IKadenaSettings settings,
           IOrderDataFactory orderDataFactory,
           IKenticoResourceService resources,
           IDeliveryEstimationDataService deliveryEstimationData
         )
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.kenticoOrder = kenticoOrder ?? throw new ArgumentNullException(nameof(kenticoOrder));
            this.shoppingCart = shoppingCart ?? throw new ArgumentNullException(nameof(shoppingCart));
            this.orderCartItems = orderCartItems ?? throw new ArgumentNullException(nameof(orderCartItems));
            this.kenticoCustomer = kenticoCustomer ?? throw new ArgumentNullException(nameof(kenticoCustomer));
            this.kenticoLog = kenticoLog ?? throw new ArgumentNullException(nameof(kenticoLog));
            this.taxService = taxService ?? throw new ArgumentNullException(nameof(taxService));
            this.siteProvider = site ?? throw new ArgumentNullException(nameof(site));
            this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
            this.orderDataFactory = orderDataFactory ?? throw new ArgumentNullException(nameof(orderDataFactory));
            this.resources = resources ?? throw new ArgumentNullException(nameof(resources));
            this.deliveryEstimationData = deliveryEstimationData ?? throw new ArgumentNullException(nameof(deliveryEstimationData));
        }

        public async Task<OrderDTO> GetSubmitOrderData(SubmitOrderRequest request)
        {
            Customer customer = kenticoCustomer.GetCurrentCustomer();

            var notificationEmails = request.EmailConfirmation.Union(new[] { customer.Email });

            var shippingAddress = shoppingCart.GetCurrentCartShippingAddress();
            var billingAddress = shoppingCart.GetDefaultBillingAddress();
            var site = siteProvider.GetKenticoSite();
            var paymentMethod = shoppingCart.GetPaymentMethod(request.PaymentMethod.Id);
            var cartItems = orderCartItems.GetOrderCartItems();
            var currency = siteProvider.GetSiteCurrency();
            var totals = shoppingCart.GetShoppingCartTotals();
            var shippingCost = shoppingCart.GetCurrentCartShippingCost();
            totals.TotalTax = await taxService.EstimateTax(shippingAddress, totals.TotalItemsPrice, (decimal)shippingCost);
            totals.TotalItemsPrice = cartItems.Sum(i => i.TotalPrice); // cannot round whole sum, need to sum rounded numbers

            var pricedItemsTax = totals.TotalTax;
            if (cartItems.Any(c => !c.SendPriceToErp))
            {
                var pricedItemsPrice = cartItems
                    .Where(c => c.SendPriceToErp)
                    .Sum(c => c.TotalPrice);

                pricedItemsTax = await taxService.EstimateTax(shippingAddress, pricedItemsPrice, (decimal)shippingCost);
            }

            if (string.IsNullOrWhiteSpace(customer.Company))
            {
                customer.Company = settings.DefaultCustomerCompanyName;
            }

            var orderDto = new OrderDTO()
            {
                BillingAddress = orderDataFactory.CreateBillingAddress(billingAddress),
                ShippingAddressSource = GetSourceAddressForDeliveryEstimation(),
                ShippingAddressDestination = mapper.Map<AddressDTO>(shippingAddress),
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
                Totals = new TotalsDto
                {
                    Price = totals.TotalItemsPrice,
                    Shipping = totals.TotalShipping,
                    Tax = totals.TotalTax,
                    PricedItemsTax = pricedItemsTax
                },
                Items = cartItems.Select(item => MapCartItemTypeToOrderItemType(item)),
                NotificationsData = notificationEmails.Select(e => new NotificationInfoDto
                {
                    Email = e,
                    Language = customer.PreferredLanguage
                })
            };

            // If only mailing list items in cart, we are not picking any delivery option
            if (!cartItems.All( i => ProductTypes.IsOfType( i.ProductType, ProductTypes.MailingProduct)))
            {
                var deliveryMethod = shoppingCart.GetShippingOption(request.DeliveryMethod);
                orderDto.ShippingOption = new ShippingOptionDTO()
                {
                    KenticoShippingOptionID = deliveryMethod.Id,
                    CarrierCode = deliveryMethod.SAPName,
                    ShippingCompany = deliveryMethod.CarrierCode,
                    ShippingService = deliveryMethod.Service.Replace("#", ""),
                    RequestedDeliveryDate = shoppingCart.GetRequestedDeliveryDate()
                };
            }

            return orderDto;
        }

        private OrderItemDTO MapCartItemTypeToOrderItemType(OrderCartItem item)
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

        public AddressDTO GetSourceAddressForDeliveryEstimation()
        {
            return mapper.Map<AddressDTO>(deliveryEstimationData.GetSourceAddress());
        }
    }
}
