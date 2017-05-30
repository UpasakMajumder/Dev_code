using Kadena.WebAPI.Contracts;
using Kadena.WebAPI.Models;
using AutoMapper;
using System.Linq;
using System.Collections.Generic;
using CMS.Ecommerce;
using CMS.Helpers;

namespace Kadena.WebAPI.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IMapper mapper;
        private readonly IKenticoProviderService kenticoProvider;

        public ShoppingCartService(IMapper mapper, IKenticoProviderService kenticoProvider)
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
                orderedMethods.Add(creditCardMethod);
            }

            var payPalMethod = allMethods.Where(m => m.ClassName.Contains("PayPal")).FirstOrDefault();
            if (payPalMethod != null)
            {
                orderedMethods.Add(payPalMethod);
            }

            var purchaseOrderMethod = allMethods.Where(m => m.ClassName.Contains("PurchaseOrder")).FirstOrDefault();
            if (purchaseOrderMethod != null)
            {
                purchaseOrderMethod.HasInput = true;
                purchaseOrderMethod.InputPlaceholder = "Insert your PO number";
                orderedMethods.Add(purchaseOrderMethod);
            }

            return orderedMethods;
        }

        public CheckoutPage SelectShipipng(int id)
        {
            var cart = ECommerceContext.CurrentShoppingCart;

            if (cart.ShoppingCartShippingOptionID != id)
            {
                cart.ShoppingCartShippingOptionID = id;
                //ComponentEvents.RequestEvents.RaiseEvent(sender, e, SHOPPING_CART_CHANGED);
            }

            var checkoutPage = GetCheckoutPage();
            checkoutPage.DeliveryMethod.CheckMethod(id);
            return checkoutPage;
        }

        public CheckoutPage SelectAddress(int id)
        {
            var cart = ECommerceContext.CurrentShoppingCart;

            if (cart.ShoppingCartShippingAddress == null || cart.ShoppingCartShippingAddress.AddressID != id)
            {
                var address = AddressInfoProvider.GetAddressInfo(id);
                cart.ShoppingCartShippingAddress = address;
            }

            var checkoutPage = GetCheckoutPage();
            checkoutPage.DeliveryAddresses.CheckAddress(id);
            return checkoutPage;

        }
    }
}