using Kadena.WebAPI.Contracts;
using Kadena.WebAPI.Models;
using AutoMapper;
using System.Linq;
using System.Collections.Generic;

namespace Kadena.WebAPI.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IMapper mapper;
        private readonly ICMSProviderService kenticoProvider;
        private readonly IResourceStringService resources;

        public ShoppingCartService(IMapper mapper, ICMSProviderService kenticoProvider, IResourceStringService resources)
        {
            this.mapper = mapper;
            this.kenticoProvider = kenticoProvider;
            this.resources = resources;
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
                    Items = totals.ToList()
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
    }
}