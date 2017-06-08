using Kadena.WebAPI.Contracts;
using CMS.Ecommerce;
using Kadena.WebAPI.Models;
using AutoMapper;
using System.Linq;
using CMS.SiteProvider;
using CMS.Helpers;
using System;
using CMS.DataEngine;
using CMS.Globalization;

namespace Kadena.WebAPI.Services
{
    public class KenticoProviderService : IKenticoProviderService
    {
        private readonly IMapper mapper;
        private readonly IKenticoResourceService resources;

        public KenticoProviderService(IMapper mapper, IKenticoResourceService resources)
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

        public DeliveryCarrier[] GetShippingCarriers()
        {
            var shippingOptions = GetShippingOptions();
            var carriers = CarrierInfoProvider.GetCarriers(SiteContext.CurrentSiteID).ToArray();

            var deliveryMethods = mapper.Map<DeliveryCarrier[]>(carriers);

            foreach (DeliveryCarrier dm in deliveryMethods)
            {
                dm.SetShippingOptions(shippingOptions);
                SetShippingProviderIcon(dm);
            }

            return deliveryMethods;
        }

        /// <summary>
        /// Hardcoded until finding some convinient way to configure it in Kentico
        /// </summary>
        private void SetShippingProviderIcon(DeliveryCarrier dm)
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

        public DeliveryOption[] GetShippingOptions()
        {
            var services = ShippingOptionInfoProvider.GetShippingOptions(SiteContext.CurrentSiteID).ToArray();
            var result = mapper.Map<DeliveryOption[]>(services);
            GetShippingPrice(result);
            return result;
        }

        public DeliveryOption GetShippingOption(int id)
        {
            var service = ShippingOptionInfoProvider.GetShippingOptionInfo(id);
            var result = mapper.Map<DeliveryOption>(service);
            var carrier = CarrierInfoProvider.GetCarrierInfo(service.ShippingOptionCarrierID);
            result.CarrierCode = carrier.CarrierName;
            return result;
        }

        private void GetShippingPrice(DeliveryOption[] services)
        {
            // this method's approach comes from origial kentico webpart (ShippingSeletion)
            int originalCartShippingId = ECommerceContext.CurrentShoppingCart.ShoppingCartShippingOptionID;

            foreach (var s in services)
            {
                var cart = ECommerceContext.CurrentShoppingCart;
                cart.ShoppingCartShippingOptionID = s.Id;
                s.PriceAmount = cart.TotalShipping;
                s.Price = String.Format("$ {0:#,0.00}", ECommerceContext.CurrentShoppingCart.TotalShipping);

                if (cart.TotalShipping == 0.0d && !s.IsCustomerPrice)
                {
                    s.Disabled = true;
                }
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
                TotalTax = 0.0d //TODO call tax service
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

        public int GetCurrentCartShippingOptionId()
        {
            return ECommerceContext.CurrentShoppingCart.ShoppingCartShippingOptionID;
        }

        public string GetResourceString(string name)
        {
            return ResHelper.GetString(name, useDefaultCulture: true);
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
                UserID = customer.CustomerUserID,
                Company = customer.CustomerCompany
            };
        }

        public OrderItem[] GetShoppingCartOrderItems()
        {
            var items = ECommerceContext.CurrentShoppingCart.CartItems;
            var result = items.Select(i => new OrderItem()
            {
                DesignFilePath = i.GetValue("ArtworkLocation", /*string.Empty*/"defaultartworklocation"),// TODO via calling service for templated
                MailingListId = Guid.NewGuid(), // seem to be redundant parameter, microservise doesn't use it
                OrderItemType = i.GetValue("ProductType", /*string.Empty*/ "KDA.StaticProduct"), // TODO
                SKUName = i.SKU?.SKUName,
                SKUNumber = i.SKU?.SKUNumber,
                KenticoSKUId = i.SKUID,
                TotalPrice = i.TotalPrice,
                TotalTax = i.TotalTax, //TODO tax
                UnitPrice = i.UnitPrice,
                UnitCount = i.CartItemUnits,
                UnitOfMeasure = "EA"
            }
            ).ToArray();

            return result;
        }

        public CartItem[] GetShoppingCartItems()
        {
            var items = ECommerceContext.CurrentShoppingCart.CartItems;
            var result = items.Select(i => new CartItem()
            {
                Id = i.CartItemID,
                Image = URLHelper.GetAbsoluteUrl(i.SKU.SKUImagePath),
                IsEditable = false,
                Quantity = i.CartItemUnits,
                Price = i.CartItemPrice * i.CartItemUnits,
                PricePrefix = resources.GetResourceString("Kadena.Checkout.ItemPricePrefix"),
                QuantityPrefix = resources.GetResourceString("Kadena.Checkout.QuantityPrefix"),
                Delivery = "", //TODO not known yet
                IsMailingList = false, //TODO
                MailingList = "Mailing list", //TODO
                Template = "Template" // TODO
            }
            ).ToArray();

            return result;
        }

        /// <summary>
        /// Inspired by \CMS\CMSModules\Ecommerce\Controls\Checkout\CartItemRemove.ascx.cs
        /// </summary>
        public void RemoveCartItem(int id)
        {
            var cart = ECommerceContext.CurrentShoppingCart;
            var item = cart.CartItems.FirstOrDefault(i => i.CartItemID == id);

            if (item == null)
                return;

            // Delete all the children from the database if available        
            foreach (ShoppingCartItemInfo scii in cart.CartItems)
            {
                if ((scii.CartItemBundleGUID == item.CartItemGUID) || (scii.CartItemParentGUID == item.CartItemGUID))
                {
                    ShoppingCartItemInfoProvider.DeleteShoppingCartItemInfo(scii);
                }
            }

            // Deletes the CartItem from the database
            ShoppingCartItemInfoProvider.DeleteShoppingCartItemInfo(item.CartItemGUID);
            // Delete the CartItem form the shopping cart object (session)
            ShoppingCartInfoProvider.RemoveShoppingCartItem(cart, item.CartItemGUID);

            // Recalculate shopping cart
            ShoppingCartInfoProvider.EvaluateShoppingCart(cart);
        }

        public void SetCartItemQuantity(int id, int quantity)
        {
            if (quantity < 1)
                return;

            var cart = ECommerceContext.CurrentShoppingCart;

            var item = ECommerceContext.CurrentShoppingCart.CartItems.Where(i => i.CartItemID == id).FirstOrDefault();
            if (item != null)
            {
                ShoppingCartItemInfoProvider.UpdateShoppingCartItemUnits(item, quantity);
                cart.InvalidateCalculations();
                ShoppingCartInfoProvider.EvaluateShoppingCart(cart);
            }
        }
    }
}