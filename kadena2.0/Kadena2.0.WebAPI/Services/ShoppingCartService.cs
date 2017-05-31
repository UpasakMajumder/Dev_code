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

        public ShoppingCartService(IMapper mapper, ICMSProviderService kenticoProvider)
        {
            this.mapper = mapper;
            this.kenticoProvider = kenticoProvider;
        }

        public CheckoutPage GetCheckoutPage()
        {
            var addresses = kenticoProvider.GetCustomerAddresses();
            var carriers = kenticoProvider.GetShippingCarriers();
            var totals = kenticoProvider.GetShoppingCartTotals();
            var paymentMethods = kenticoProvider.GetPaymentMethods();

            return new CheckoutPage()
            {
                DeliveryAddresses = new DeliveryAddresses()
                {
                    AddAddressLabel = "New address",
                    Title = "Delivery",
                    Description = "Products will be delivered to selected address by",
                    items = addresses.ToList()
                },

                DeliveryMethod = new DeliveryMethods()
                {
                    Title = "Delivery",
                    Description = "Select delivery carrier and option",
                    items = carriers.ToList(),
                },

                Totals = new Totals()
                {
                    Title = "Total",
                    Description = null,
                    Items = totals.ToList()
                },

                PaymentMethods = new PaymentMethods()
                {
                    Title = "Payment",
                    Description = null,
                    Items  = OrderPaymentMethods(paymentMethods)
                },

                SubmitLabel = "Place order"
            };
        }

        public List<PaymentMethod> OrderPaymentMethods(PaymentMethod[] allMethods)
        {
            var orderedMethods = new List<PaymentMethod>();

            var creditCardMethod = allMethods.Where(m => m.ClassName.Contains("CreditCard")).FirstOrDefault();
            if (creditCardMethod != null)
            {
                creditCardMethod.Disabled = true;
                orderedMethods.Add(creditCardMethod);
            }

            var payPalMethod = allMethods.Where(m => m.ClassName.Contains("PayPal")).FirstOrDefault();
            if (payPalMethod != null)
            {
                payPalMethod.Disabled = true;
                orderedMethods.Add(payPalMethod);
            }

            var purchaseOrderMethod = allMethods.Where(m => m.ClassName.Contains("PurchaseOrder")).FirstOrDefault();
            if (purchaseOrderMethod != null)
            {
                purchaseOrderMethod.Disabled = false;
                purchaseOrderMethod.HasInput = true;
                purchaseOrderMethod.InputPlaceholder = "Insert your PO number";
                orderedMethods.Add(purchaseOrderMethod);
            }

            return orderedMethods;
        }

        public CheckoutPage SelectShipipng(int id)
        {
            kenticoProvider.SelectShipping(id);

            var checkoutPage = GetCheckoutPage();
            checkoutPage.DeliveryMethod.CheckMethod(id);
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