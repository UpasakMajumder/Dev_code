using Kadena.WebAPI.Contracts;
using Kadena.WebAPI.Models;
using AutoMapper;
using System.Linq;
using System.Collections.Generic;
using Kadena.WebAPI.Models.SubmitOrder;
using System.Threading.Tasks;
using Kadena.Dto.SubmitOrder;
using PaymentMethod = Kadena.WebAPI.Models.PaymentMethod;
using System;

namespace Kadena.WebAPI.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IMapper mapper;
        private readonly ICMSProviderService kenticoProvider;
        private readonly IResourceService resources;
        private readonly IOrderServiceCaller orderCaller;

        public ShoppingCartService(IMapper mapper, ICMSProviderService kenticoProvider, IResourceService resources, IOrderServiceCaller orderCaller)
        {
            this.mapper = mapper;
            this.kenticoProvider = kenticoProvider;
            this.resources = resources;
            this.orderCaller = orderCaller;
        }

        public CheckoutPage GetCheckoutPage()
        {
            var addresses = kenticoProvider.GetCustomerAddresses();
            var carriers = kenticoProvider.GetShippingCarriers();
            var totals = kenticoProvider.GetShoppingCartTotals();

            var paymentMethods = kenticoProvider.GetPaymentMethods();

            var checkoutPage = new CheckoutPage()
            {
                DeliveryAddresses = new DeliveryAddresses()
                {
                    AddAddressLabel = resources.GetResourceString("Kadena.Checkout.NewAddress"),
                    Title = resources.GetResourceString("Kadena.Checkout.DeliveryAddress.Title"),
                    Description = resources.GetResourceString("Kadena.Checkout.DeliveryDescription"),
                    items = addresses.ToList()
                },

                DeliveryMethods = new DeliveryMethods()
                {
                    Title = resources.GetResourceString("Kadena.Checkout.Delivery.Title"),
                    Description = resources.GetResourceString("Kadena.Checkout.DeliveryMethodDescription"),
                    items = carriers.ToList()
                },

                Totals = new Totals()
                {
                    Title = resources.GetResourceString("Kadena.Checkout.Totals.Title"),
                    Description = null, // resources.GetResourceString("Kadena.Checkout.Totals.Description"), if needed
                    Items = MapTotals(totals)
                },

                PaymentMethods = new PaymentMethods()
                {
                    Title = resources.GetResourceString("Kadena.Checkout.Payment.Title"),
                    Description = null, // resources.GetResourceString("Kadena.Checkout.Payment.Description"), if needed
                    Items = OrderPaymentMethods(paymentMethods)
                },

                SubmitLabel = resources.GetResourceString("Kadena.Checkout.ButtonPlaceOrder"),
                ValidationMessage = resources.GetResourceString("Kadena.Checkout.ValidationError")
            };

            CheckCurrentOrDefaultAddress(checkoutPage);
            CheckCurrentOrDefaultShipping(checkoutPage);
            checkoutPage.PaymentMethods.CheckDefault();
            checkoutPage.DeliveryMethods.UpdateSummaryText(
                    resources.GetResourceString("Kadena.Checkout.ShippingPriceFrom"),
                    resources.GetResourceString("Kadena.Checkout.ShippingPrice"),
                    resources.GetResourceString("Kadena.Checkout.CannotBeDelivered")
                );
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
                    Value = String.Format("$ {0:#,0.00}", 0)
                },
                new Total()
                {
                    Title = resources.GetResourceString("Kadena.Checkout.Totals.Tax"),
                    Value = String.Format("$ {0:#,0.00}", 0)
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
                kenticoProvider.SetShoppingCartAddres(defaultAddressId);
                page.DeliveryAddresses.CheckAddress(defaultAddressId);
            }
        }

        private void CheckCurrentOrDefaultShipping(CheckoutPage page)
        { 
            int currentShippingMethod = kenticoProvider.GetCurrentCartShippingMethodId();
            if (currentShippingMethod != 0)
            {
                page.DeliveryMethods.CheckMethod(currentShippingMethod);
            }
            else
            {
                int defaultMethodId = page.DeliveryMethods.GetDefaultMethodId();
                kenticoProvider.SelectShipping(defaultMethodId);
                page.DeliveryMethods.CheckMethod(defaultMethodId);
            }
        }

        public List<PaymentMethod> OrderPaymentMethods(PaymentMethod[] allMethods)
        {
            var orderedMethods = new List<PaymentMethod>();

            var creditCardMethod = allMethods.Where(m => m.ClassName.Contains("CreditCard")).FirstOrDefault();
            if (creditCardMethod != null)
            {
                creditCardMethod.Icon = "credit-card";
                creditCardMethod.Disabled = true;
                orderedMethods.Add(creditCardMethod);
            }

            var payPalMethod = allMethods.Where(m => m.ClassName.Contains("PayPal")).FirstOrDefault();
            if (payPalMethod != null)
            {
                payPalMethod.Icon = "paypal-payment";
                payPalMethod.Disabled = true;
                orderedMethods.Add(payPalMethod);
            }

            var purchaseOrderMethod = allMethods.Where(m => m.ClassName.Contains("PurchaseOrder")).FirstOrDefault();
            if (purchaseOrderMethod != null)
            {
                purchaseOrderMethod.Icon = "order-payment";
                purchaseOrderMethod.Disabled = false;
                purchaseOrderMethod.HasInput = true;
                purchaseOrderMethod.InputPlaceholder = resources.GetResourceString("Kadena.Checkout.InsertPONumber");
                orderedMethods.Add(purchaseOrderMethod);
            }

            return orderedMethods;
        }

        public CheckoutPage SelectShipipng(int id)
        {
            kenticoProvider.SelectShipping(id);

            var checkoutPage = GetCheckoutPage();
            checkoutPage.DeliveryMethods.CheckMethod(id);
            return checkoutPage;
        }

        public CheckoutPage SelectAddress(int id)
        {
            kenticoProvider.SetShoppingCartAddres(id);

            var checkoutPage = GetCheckoutPage();
            checkoutPage.DeliveryAddresses.CheckAddress(id);
            return checkoutPage;
        }

        public async Task<SubmitOrderResult> SubmitOrder(SubmitOrderRequest request)
        {
            string serviceEndpoint = resources.GetSettingsKey("KDA_OrderServiceEndpoint");
            var orderData = GetSubmitOrderData(request.DeliveryMethod, request.PaymentMethod.Id, request.PaymentMethod.Invoice);

            var serviceResult = await orderCaller.SubmitOrder(serviceEndpoint, orderData);

            // todo process service result

            return mapper.Map<SubmitOrderResult>(serviceResult);
        }


        private OrderDTO GetSubmitOrderData(int deliveryMethodId, int paymentMethodId, string invoice)
        {
            var shippingAddress = kenticoProvider.GetCurrentCartShippingAddress();
            var billingAddress = kenticoProvider.GetDefaultBillingAddress();
            var customer = kenticoProvider.GetCurrentCustomer();
            var deliveryMethod = kenticoProvider.GetShippingOption(deliveryMethodId);
            var site = resources.GetKenticoSite();
            var totals = kenticoProvider.GetShoppingCartTotals();
            var paymentMethod = kenticoProvider.GetPaymentMethod(paymentMethodId);

            return new OrderDTO()
            {
                BillingAddress = new AddressDTO()
                {
                    AddressLine1 = billingAddress.Street.Count > 0 ? billingAddress.Street[0] : null,
                    AddressLine2 = billingAddress.Street.Count > 1 ? billingAddress.Street[1] : null,
                    City = billingAddress.City,
                    State = billingAddress.State,
                    Zip = billingAddress.Zip,
                    Country = billingAddress.Country
               },
               ShippingAddress = new AddressDTO()
               {
                   AddressLine1 = shippingAddress.Street.Count > 0 ? shippingAddress.Street[0] : null,
                   AddressLine2 = shippingAddress.Street.Count > 1 ? shippingAddress.Street[1] : null,
                   City = shippingAddress.City,
                   State = shippingAddress.State,
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
                  CarrierCode = deliveryMethod.CarrierCode,
                  ShippingCompany = deliveryMethod.Title,
                  ShippingService = deliveryMethod.Service
               },
               Site = new SiteDTO()
               {
                   KenticoSiteID = site.Id,
                   KenticoSiteName = site.Name
               },
               TotalPrice = totals.TotalItemsPrice,
               TotalShipping = totals.TotalShipping,
               TotalTax = totals.TotalTax
            };
        }
    }
}
 