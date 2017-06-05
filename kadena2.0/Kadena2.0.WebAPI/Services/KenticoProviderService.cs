using Kadena.WebAPI.Contracts;
using CMS.Ecommerce;
using Kadena.WebAPI.Models;
using AutoMapper;
using System.Linq;
using CMS.SiteProvider;
using CMS.Helpers;
using System;
using System.Collections.Generic;
using CMS.DataEngine;
using CMS.Globalization;

namespace Kadena.WebAPI.Services
{
    public class KenticoProviderService : ICMSProviderService
    {
        private readonly IMapper mapper;
        private readonly IResourceService resources;

        public KenticoProviderService(IMapper mapper, IResourceService resources)
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

        public DeliveryAddress GetCurrentCartShippingAddress()
        {
            var address = ECommerceContext.CurrentShoppingCart.ShoppingCartShippingAddress;

            if (address == null)
                return null;

            return mapper.Map<DeliveryAddress>(address);
        }

        public BillingAddress GetDefaultBillingAddress()
        {
            var streets = new[]
            {
                SettingsKeyInfoProvider.GetValue(SiteContext.CurrentSiteName + ".KDA_EstimateDeliveryPrice_SenderAddressLine1"),
                SettingsKeyInfoProvider.GetValue(SiteContext.CurrentSiteName + ".KDA_EstimateDeliveryPrice_SenderAddressLine2")
            }.Where(i => !string.IsNullOrEmpty(i)).ToList();

            string countryName = SettingsKeyInfoProvider.GetValue(SiteContext.CurrentSiteName + ".KDA_EstimateDeliveryPrice_SenderCountry");
            string stateName = SettingsKeyInfoProvider.GetValue(SiteContext.CurrentSiteName + ".KDA_EstimateDeliveryPrice_SenderState");
            int countryId = CountryInfoProvider.GetCountryInfoByCode(countryName).CountryID;
            int stateId = StateInfoProvider.GetStateInfoByCode(stateName).StateID;

            return new BillingAddress()
            {
                Street = streets,
                City = SettingsKeyInfoProvider.GetValue(SiteContext.CurrentSiteName + ".KDA_EstimateDeliveryPrice_SenderCity"),
                Country = countryName,
                CountryId = countryId,
                Zip = SettingsKeyInfoProvider.GetValue(SiteContext.CurrentSiteName + ".KDA_EstimateDeliveryPrice_SenderPostal"),
                State = stateName,
                StateId = stateId
            };
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

        public DeliveryService GetShippingOption(int id)
        {
            var service = ShippingOptionInfoProvider.GetShippingOptionInfo(id);
            var result = mapper.Map<DeliveryService>(service);
            var carrier = CarrierInfoProvider.GetCarrierInfo(service.ShippingOptionCarrierID);
            result.CarrierCode = carrier.CarrierName;
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

        public PaymentMethod GetPaymentMethod(int id)
        {
            var method = PaymentOptionInfoProvider.GetPaymentOptionInfo(id);
            return mapper.Map<PaymentMethod>(method);
        }

        public ShoppingCartTotals GetShoppingCartTotals()
        {
            return new ShoppingCartTotals()
            {
                TotalItemsPrice = ECommerceContext.CurrentShoppingCart.TotalItemsPrice,
                TotalShipping = ECommerceContext.CurrentShoppingCart.TotalShipping,
                TotalPrice = ECommerceContext.CurrentShoppingCart.TotalPrice,
                TotalTax = 0.0d //TODO
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


        public Customer GetCurrentCustomer()
        {
            var customer = ECommerceContext.CurrentCustomer;

            if (customer == null)
                return null;

            return new Customer()
            {
                Id = customer.CustomerID,
                FirstName = customer.CustomerFirstName,
                LastName = customer.CustomerLastName,
                Email = customer.CustomerEmail,
                CustomerNumber = customer.CustomerGUID.ToString(),
                Phone = customer.CustomerPhone,
                UserID = customer.CustomerUserID
            };
        }

        public ShoppingCartItem[] GetShoppingCartItems()
        {
            var items = ECommerceContext.CurrentShoppingCart.CartItems;
            var result = items.Select(i => new ShoppingCartItem()
                {
                    DesignFilePath = "design/file/path",// TODO
                    MailingListId = Guid.NewGuid(), // TODO
                    OrderItemType = "", // TODO
                    SKUName = i.SKU?.SKUName,
                    SKUNumber = i.SKU?.SKUNumber,
                    KenticoSKUId = i.SKUID,
                    TotalPrice = i.TotalPrice,
                    TotalTax = i.TotalTax, //TODO
                    UnitPrice = i.UnitPrice,
                    UnitCount = i.CartItemUnits,
                    UnitOfMeasure = "EA" 
                }
            ).ToArray();

            return result;
        }
    }
}