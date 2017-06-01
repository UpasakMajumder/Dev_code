using Kadena.WebAPI.Contracts;
using CMS.Ecommerce;
using Kadena.WebAPI.Models;
using AutoMapper;
using System.Linq;
using CMS.SiteProvider;
using CMS.Helpers;
using System;

namespace Kadena.WebAPI.Services
{
    public class KenticoProviderService : ICMSProviderService
    {
        private readonly IMapper mapper;
        private readonly IResourceStringService resources;

        public KenticoProviderService(IMapper mapper, IResourceStringService resources)
        {
            this.mapper = mapper;
            this.resources = resources;
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
            {
                dm.SetShippingOptions(shippingOptions);
                SetShippingProviderIcon(dm);
            }

            return deliveryMethods;
        }

        /// <summary>
        /// Hardcoded until finding some convinient way to configure it in Kentico
        /// </summary>
        private void SetShippingProviderIcon(DeliveryMethod dm)
        {
            if (dm.Title.ToLower().Contains("fedex"))
            {
                dm.Icon = "fedex-delivery";
            }
            else if (dm.Title.ToLower().Contains("usps"))
            {
                dm.Icon = "usps-delivery";
            }
            else if (dm.Title.ToLower().Contains("ups"))
            {
                dm.Icon = "ups-delivery";
            }
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
                s.PriceAmount = ECommerceContext.CurrentShoppingCart.TotalShipping;
                s.Price = String.Format("$ {0:#,0.00}", ECommerceContext.CurrentShoppingCart.TotalShipping);
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
                    Title = resources.GetResourceString("Kadena.Checkout.Totals.Summary"),
                    Value = String.Format("$ {0:#,0.00}", ECommerceContext.CurrentShoppingCart.TotalItemsPrice)
                },
                new Total()
                {
                    Title = resources.GetResourceString("Kadena.Checkout.Totals.Shipping"),
                    Value = String.Format("$ {0:#,0.00}", ECommerceContext.CurrentShoppingCart.TotalShipping)
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
                    Value = String.Format("$ {0:#,0.00}", ECommerceContext.CurrentShoppingCart.TotalPrice)
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
                cart.SubmitChanges(true);
            }
        }

        public void SelectShipping(int shippingOptionId)
        {
            var cart = ECommerceContext.CurrentShoppingCart;

            if (cart.ShoppingCartShippingOptionID != shippingOptionId)
            {
                cart.ShoppingCartShippingOptionID = shippingOptionId;
                cart.SubmitChanges(true);
            }
        }

        public int GetCurrentCartAddresId()
        {
            var address = ECommerceContext.CurrentShoppingCart.ShoppingCartShippingAddress;

            if (address == null)
                return 0;

            return address.AddressID;
        }

        public int GetCurrentCartShippingMethodId()
        {
            return ECommerceContext.CurrentShoppingCart.ShoppingCartShippingOptionID;
        }

        public string GetResourceString(string name)
        {
            return ResHelper.GetString(name, useDefaultCulture:true);
        }
    }
}