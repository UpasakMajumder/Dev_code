using Kadena.WebAPI.Contracts;
using Kadena.WebAPI.Models;
using AutoMapper;
using System.Linq;
using System.Collections.Generic;
using Kadena.WebAPI.Models.SubmitOrder;
using System.Threading.Tasks;
using PaymentMethod = Kadena.WebAPI.Models.PaymentMethod;
using System;
using Kadena2.MicroserviceClients.Contracts;
using Kadena.Dto.SubmitOrder.MicroserviceRequests;
using Kadena2.MicroserviceClients.MicroserviceRequests;

namespace Kadena.WebAPI.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IMapper mapper;
        private readonly IKenticoProviderService kenticoProvider;
        private readonly IKenticoResourceService resources;
        private readonly IOrderSubmitClient orderClient;
        private readonly IKenticoLogger kenticoLog;
        private readonly ITaxEstimationService taxCalculator;

        public ShoppingCartService(IMapper mapper, 
                                   IKenticoProviderService kenticoProvider,
                                   IKenticoResourceService resources, 
                                   IOrderSubmitClient orderClient, 
                                   ITaxEstimationService taxCalculator, 
                                   IKenticoLogger kenticoLog)
        {
            this.mapper = mapper;
            this.kenticoProvider = kenticoProvider;
            this.resources = resources;
            this.orderClient = orderClient;
            this.taxCalculator = taxCalculator;
            this.kenticoLog = kenticoLog;
        }

        public async Task<CheckoutPage> GetCheckoutPage()
        {
            var addresses = kenticoProvider.GetCustomerAddresses();
            var carriers = kenticoProvider.GetShippingCarriers();            
            var paymentMethods = kenticoProvider.GetPaymentMethods();
            var cartItems = kenticoProvider.GetShoppingCartItems();
            var items = cartItems.Length == 1 ? "item" : "items"; // todo configurable

            var checkoutPage = new CheckoutPage()
            {
                Products = new CartItems()
                {
                    Number = $"You have {cartItems.Length} {items} in your shopping cart",
                    Items = cartItems.ToList()
                },

                DeliveryAddresses = new DeliveryAddresses()
                {
                    IsDeliverable = true,
                    UnDeliverableText = resources.GetResourceString("Kadena.Checkout.UndeliverableText"),
                    AddAddressLabel = resources.GetResourceString("Kadena.Checkout.NewAddress"),
                    Title = resources.GetResourceString("Kadena.Checkout.DeliveryAddress.Title"),
                    Description = resources.GetResourceString("Kadena.Checkout.DeliveryDescription"),
                    EmptyMessage = resources.GetResourceString("Kadena.Checkout.NoAddressesMessage"),
                    items = addresses.ToList()
                },

                DeliveryMethods = new DeliveryCarriers()
                {
                    Title = resources.GetResourceString("Kadena.Checkout.Delivery.Title"),
                    Description = resources.GetResourceString("Kadena.Checkout.DeliveryMethodDescription"),
                    items = carriers.ToList()
                },

                Totals = new Totals()
                {
                    Title = resources.GetResourceString("Kadena.Checkout.Totals.Title"),
                    Description = null, // resources.GetResourceString("Kadena.Checkout.Totals.Description"), if needed
                    // will be assigned after computing TotalTax, which may be changed aftes selecting default shipping or address
                },

                PaymentMethods = new PaymentMethods()
                {
                    IsPayable = true,
                    UnPayableText = resources.GetResourceString("Kadena.Checkout.UnpayableText"),
                    Title = resources.GetResourceString("Kadena.Checkout.Payment.Title"),
                    Description = null, // resources.GetResourceString("Kadena.Checkout.Payment.Description"), if needed
                    Items = ArrangePaymentMethods(paymentMethods)
                },

                SubmitLabel = resources.GetResourceString("Kadena.Checkout.ButtonPlaceOrder"),
                ValidationMessage = resources.GetResourceString("Kadena.Checkout.ValidationError")
            };

            checkoutPage.DeliveryMethods.RemoveCarriersWithoutOptions();
            CheckCurrentOrDefaultAddress(checkoutPage);
            CheckCurrentOrDefaultShipping(checkoutPage);
            checkoutPage.PaymentMethods.CheckDefault();
            checkoutPage.PaymentMethods.CheckPayability();
            checkoutPage.DeliveryMethods.UpdateSummaryText(
                    resources.GetResourceString("Kadena.Checkout.ShippingPriceFrom"),
                    resources.GetResourceString("Kadena.Checkout.ShippingPrice"),
                    resources.GetResourceString("Kadena.Checkout.CannotBeDelivered"),
                    resources.GetResourceString("Kadena.Checkout.CustomerPrice")
                );
            checkoutPage.SetDisplayType();

            var totals = kenticoProvider.GetShoppingCartTotals();
            totals.TotalTax = await EstimateTotalTax();
            checkoutPage.Totals.Items = MapTotals(totals);

            return checkoutPage;
        }

        private List<Total> MapTotals(ShoppingCartTotals totals)
        {
            return new Total[]
            {
                new Total()
                {
                    Title = resources.GetResourceString("Kadena.Checkout.Totals.Summary"),
                    Value = String.Format("$ {0:#,0.00}", totals.TotalItemsPrice)
                },
                new Total()
                {
                    Title = resources.GetResourceString("Kadena.Checkout.Totals.Shipping"),
                    Value = String.Format("$ {0:#,0.00}", totals.TotalShipping)
                },
                new Total()
                {
                    Title = resources.GetResourceString("Kadena.Checkout.Totals.Subtotal"),
                    Value = String.Format("$ {0:#,0.00}", totals.Subtotal)
                },
                new Total()
                {
                    Title = resources.GetResourceString("Kadena.Checkout.Totals.Tax"),
                    Value = String.Format("$ {0:#,0.00}", totals.TotalTax)
                },
                new Total()
                {
                    Title = resources.GetResourceString("Kadena.Checkout.Totals.Totals"),
                    Value = String.Format("$ {0:#,0.00}", totals.TotalPrice)
                }
            }.ToList();
        }

        private void CheckCurrentOrDefaultAddress(CheckoutPage page)
        {
            int currentAddress = kenticoProvider.GetCurrentCartAddresId();
            if (currentAddress != 0)
            {
                page.DeliveryAddresses.CheckAddress(currentAddress);
            }
            else
            {
                int defaultAddressId = page.DeliveryAddresses.GetDefaultAddressId();
                if (defaultAddressId != 0)
                {
                    kenticoProvider.SetShoppingCartAddres(defaultAddressId);
                    page.DeliveryAddresses.CheckAddress(defaultAddressId);
                }
            }
        }

        private void CheckCurrentOrDefaultShipping(CheckoutPage page)
        {
            int currentShipping = kenticoProvider.GetCurrentCartShippingOptionId();

            if (page.DeliveryMethods.IsPresent(currentShipping) && !page.DeliveryMethods.IsDisabled(currentShipping))
            {
                page.DeliveryMethods.CheckMethod(currentShipping);
            }
            else
            {
                SetDefaultShipping(page);
            }
        }

        private void SetDefaultShipping(CheckoutPage page)
        { 
            int defaultMethodId = page.DeliveryMethods.GetDefaultMethodId();
            kenticoProvider.SelectShipping(defaultMethodId);
            page.DeliveryMethods.CheckMethod(defaultMethodId);
        }

        public List<PaymentMethod> ArrangePaymentMethods(PaymentMethod[] allMethods)
        {
            var purchaseOrderMethod = allMethods.Where(m => m.ClassName.Contains("PurchaseOrder")).FirstOrDefault();
            if (purchaseOrderMethod != null)
            {
                purchaseOrderMethod.HasInput = true;
                purchaseOrderMethod.InputPlaceholder = resources.GetResourceString("Kadena.Checkout.InsertPONumber");
            }

            return allMethods.ToList();
        }

        public async Task<CheckoutPage> SelectShipipng(int id)
        {
            kenticoProvider.SelectShipping(id);
            return await GetCheckoutPage();
        }

        public async Task<CheckoutPage> SelectAddress(int id)
        {
            kenticoProvider.SetShoppingCartAddres(id);
            var checkoutPage = await GetCheckoutPage();
            checkoutPage.DeliveryAddresses.CheckAddress(id);
            return checkoutPage;
        }

        public async Task<SubmitOrderResult> SubmitOrder(SubmitOrderRequest request)
        {
            string serviceEndpoint = resources.GetSettingsKey("KDA_OrderServiceEndpoint");
            var orderData = await GetSubmitOrderData(request.DeliveryMethod, request.PaymentMethod.Id, request.PaymentMethod.Invoice);
            var serviceResultDto = await orderClient.SubmitOrder(serviceEndpoint, orderData);
            var serviceResult = mapper.Map<SubmitOrderResult>(serviceResultDto);
            var redirectUrl = $"/order-submitted?success={serviceResult.Success.ToString().ToLower()}";
            serviceResult.RedirectURL = redirectUrl;

            if (serviceResult.Success)
            {
                kenticoLog.LogInfo("Submit order", "INFORMATION", $"Order {serviceResult.Payload} sucesfully created");
                await OrderCurrentCart();
            }
            else
            {
                kenticoLog.LogError("Submit order", $"Order {serviceResult?.Payload} error. {serviceResult?.Error?.Message}");
            }

            return serviceResult;
        }


        private async Task<OrderDTO> GetSubmitOrderData(int deliveryMethodId, int paymentMethodId, string invoice)
        {
            var shippingAddress = kenticoProvider.GetCurrentCartShippingAddress();
            var billingAddress = kenticoProvider.GetDefaultBillingAddress();
            var customer = kenticoProvider.GetCurrentCustomer();
            var deliveryMethod = kenticoProvider.GetShippingOption(deliveryMethodId);
            var site = resources.GetKenticoSite();
            
            var paymentMethod = kenticoProvider.GetPaymentMethod(paymentMethodId);
            var cartItems = kenticoProvider.GetShoppingCartOrderItems();
            var currency = resources.GetSiteCurrency();
            var totals = kenticoProvider.GetShoppingCartTotals();
            totals.TotalTax = await EstimateTotalTax();
            
            if (string.IsNullOrWhiteSpace(customer.Company))
                customer.Company = resources.GetDefaultCustomerCompanyName();

            return new OrderDTO()
            {
                BillingAddress = new AddressDTO()
                {
                    AddressLine1 = billingAddress.Street.Count > 0 ? billingAddress.Street[0] : null,
                    AddressLine2 = billingAddress.Street.Count > 1 ? billingAddress.Street[1] : null,
                    City = billingAddress.City,
                    State = !string.IsNullOrEmpty(billingAddress.State)? billingAddress.State: billingAddress.Country, // fill in mandatory for countries that have no states
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
               ShippingOption = new ShippingOptionDTO()
               {
                  KenticoShippingOptionID = deliveryMethod.Id,
                  CarrierCode = deliveryMethod.Title,
                  ShippingCompany = deliveryMethod.CarrierCode,
                  ShippingService = deliveryMethod.Service.Replace("#","")
               },
               Site = new SiteDTO()
               {
                   KenticoSiteID = site.Id,
                   KenticoSiteName = site.Name
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
               Items = mapper.Map<OrderItemDTO[]>(cartItems)
            };
        }

        public async Task<CheckoutPage> ChangeItemQuantity(int id, int quantity)
        {
            kenticoProvider.SetCartItemQuantity(id, quantity);
            return await GetCheckoutPage();
        }

        public async Task<CheckoutPage> RemoveItem(int id)
        {
            kenticoProvider.RemoveCartItem(id);
            return await GetCheckoutPage();
        }

        public async Task<CheckoutPage> OrderCurrentCart()
        {
            kenticoProvider.RemoveCurrentItemsFromStock();
            kenticoProvider.RemoveCurrentItemsFromCart();
            return await GetCheckoutPage();
        }

        public async Task<double> EstimateTotalTax()
        {
            var addressFrom = kenticoProvider.GetDefaultBillingAddress();
            var addressTo = kenticoProvider.GetCurrentCartShippingAddress();
            var serviceEndpoint = resources.GetSettingsKey("KDA_TaxEstimationServiceEndpoint");
            double totalItemsPrice = kenticoProvider.GetCurrentCartTotalItemsPrice();
            double shippingCosts  = kenticoProvider.GetCurrentCartShippingCost();

            if (totalItemsPrice == 0.0d && shippingCosts == 0.0d)
            {
                // not call microservice in this case
                return 0.0d;
            }

            var taxRequest = CreateTaxCalculatorRequest(totalItemsPrice, shippingCosts, addressFrom, addressTo);
            var taxResponse = await taxCalculator.CalculateTax(serviceEndpoint, taxRequest);

            if (!taxResponse.Success)
            {
                kenticoLog.LogError("Tax estimation", $"Failed to estimate tax: {taxResponse.ErrorMessages}");
                return 0.0d;
            }

            return taxResponse.Payload;
        }

        private TaxCalculatorRequestDto CreateTaxCalculatorRequest(double totalItemsPrice, double shippingCosts, BillingAddress addressFrom, DeliveryAddress addressTo)
        {
            var taxRequest = new TaxCalculatorRequestDto()
            {
                TotalBasePrice = totalItemsPrice,
                ShipCost = shippingCosts
            };

            if (addressFrom != null)
            {
                taxRequest.ShipFromCity = addressFrom.City ?? string.Empty;
                taxRequest.ShipFromState = addressFrom.State ?? string.Empty;
                taxRequest.ShipFromZip = addressFrom.Zip ?? string.Empty;
            }

            if (addressTo != null)
            {
                taxRequest.ShipToCity = addressTo.City ?? string.Empty;
                taxRequest.ShipToState = addressTo.State ?? string.Empty;
                taxRequest.ShipToZip = addressTo.Zip ?? string.Empty;
            }

            return taxRequest;
        }
    }
}
 