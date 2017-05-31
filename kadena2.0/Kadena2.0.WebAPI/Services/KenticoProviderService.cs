using Kadena.WebAPI.Contracts;
using CMS.Ecommerce;
using Kadena.WebAPI.Models;
using AutoMapper;
using System.Linq;
using CMS.SiteProvider;
using CMS.Helpers;

namespace Kadena.WebAPI.Services
{
    public class KenticoProviderService : ICMSProviderService
    {
        private readonly IMapper mapper;
        public KenticoProviderService(IMapper mapper)
        {
            this.mapper = mapper;
        }
        
        public DeliveryAddress[] GetCustomerAddresses()
        {
            var customer = ECommerceContext.CurrentCustomer;
            var addresses = AddressInfoProvider.GetAddresses(customer.CustomerID).ToArray();
            return mapper.Map<DeliveryAddress[]>(addresses);
        }

        public DeliveryMethod[] GetShippingCarriers()
        {
            var shippingOptions = GetShippingOptions();
            var carriers = CarrierInfoProvider.GetCarriers(SiteContext.CurrentSiteID).ToArray();

            var deliveryMethods = mapper.Map<DeliveryMethod[]>(carriers);

            foreach (DeliveryMethod dm in deliveryMethods)
                dm.SetShippingOptions(shippingOptions);

            return deliveryMethods;
        }

        public DeliveryService[] GetShippingOptions()
        {
            var services = ShippingOptionInfoProvider.GetShippingOptions(SiteContext.CurrentSiteID).ToArray();
            var result = mapper.Map<DeliveryService[]>(services);
            GetShippingPrice(result);
            return result;
        }

        private void GetShippingPrice(DeliveryService[] services)
        {
            // this method's approach comes from origial kentico webpart (ShippingSeletion)
            int originalCartShippingId = ECommerceContext.CurrentShoppingCart.ShoppingCartShippingOptionID;

            foreach (var s in services)
            {
                ECommerceContext.CurrentShoppingCart.ShoppingCartShippingOptionID = s.Id;
                s.Price = ECommerceContext.CurrentShoppingCart.TotalShipping.ToString();
            }

            ECommerceContext.CurrentShoppingCart.ShoppingCartShippingOptionID = originalCartShippingId;
        }


        public PaymentMethod[] GetPaymentMethods()
        {
            var methods = PaymentOptionInfoProvider.GetPaymentOptions(SiteContext.CurrentSiteID).ToArray();
            return mapper.Map<PaymentMethod[]>(methods);
        }

        public Total[] GetShoppingCartTotals()
        {
            return new Total[]
            {
                new Total()
                {
                    Title = "Summary",
                    Value = ECommerceContext.CurrentShoppingCart.TotalItemsPrice.ToString()
                },
                new Total()
                {
                    Title = "Shipping",
                    Value = ECommerceContext.CurrentShoppingCart.TotalShipping.ToString()
                },
                new Total()
                {
                    Title = "Subtotal",
                    Value = "0"
                },
                new Total()
                {
                    Title = "Tax 8%",
                    Value = "0"
                },
                new Total()
                {
                    Title = "Totals",
                    Value = ECommerceContext.CurrentShoppingCart.TotalPrice.ToString()
                }
            };
        }

        public void SetShoppingCartAddres(int addressId)
        {
            var cart = ECommerceContext.CurrentShoppingCart;

            if (cart.ShoppingCartShippingAddress == null || cart.ShoppingCartShippingAddress.AddressID != addressId)
            {
                var address = AddressInfoProvider.GetAddressInfo(addressId);
                cart.ShoppingCartShippingAddress = address;
            }
        }

        public void SelectShipping(int shippingOptionId)
        {
            var cart = ECommerceContext.CurrentShoppingCart;

            if (cart.ShoppingCartShippingOptionID != shippingOptionId)
            {
                cart.ShoppingCartShippingOptionID = shippingOptionId;
                //ComponentEvents.RequestEvents.RaiseEvent(sender, e, SHOPPING_CART_CHANGED);
            }
        }

        public string GetResourceString(string name)
        {
            return ResHelper.GetString(name, useDefaultCulture:true);
        }
    }
}